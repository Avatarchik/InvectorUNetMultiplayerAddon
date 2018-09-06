using System.Collections;
using System.Collections.Generic;                       //for syncing of lists
using UnityEngine;
using UnityEngine.Networking;                           //to send information over the network
using UnityEngine.Networking.NetworkSystem;             //to sync animations
using Invector.vCharacterController;                    //to access "vThirdPersonController","vThirdPersonInput"
using Invector.vShooter;                                //to access "vShooterMeleeInput"
using Invector.vItemManager;                            //to access "vAmmoManager"
using Invector.vCharacterController.vActions;           //to access "vGenericAction"
using Invector.vMelee;                                  //to access "vMeleeManager"

[AddComponentMenu("Invector/Multiplayer/SetupLocalPlayer")]
[RequireComponent(typeof(NetworkIdentity))]             //needed by this script
[RequireComponent(typeof(NetworkAnimator))]             //not needed by this script, used to sync animations
[RequireComponent(typeof(MultiplayerMeleeCombatInput))]
[DisallowMultipleComponent]
public class SetupLocalPlayer : NetworkBehaviour
{

    //Components & Transforms referenced by this script
    #region Reference Components
    private Animator _anim = null;
    private Transform t_head, t_neck, t_spine, t_chest = null;
    #endregion

    //SyncVar = Update if the server variable is updated.
    #region SyncVars
    [SyncVar] private Quaternion head;
    [SyncVar] private Quaternion neck;
    [SyncVar] private Quaternion spine;
    [SyncVar] private Quaternion chest;
    [SyncVar] private Vector3 player_position = Vector3.zero;
    [SyncVar] private Quaternion player_rotation;
    #endregion

    //Network smoothing variables
    #region SyncOptions
    public float lerpRate = 90.0f;
    public float positionLerp = 45.0f;
    public float rotationLerp = 90.0f;
    #endregion

    #region Initializations
    void Start()
    {
        _anim = GetComponent<Animator>();

        if (isLocalPlayer == true)
        {
            if (GetComponent<vThirdPersonController>()) GetComponent<vThirdPersonController>().enabled = true;
            if (GetComponent<vThirdPersonInput>()) GetComponent<vThirdPersonInput>().enabled = true;
            if (GetComponent<vShooterMeleeInput>()) GetComponent<vShooterMeleeInput>().enabled = true;
            if (GetComponent<vShooterManager>()) GetComponent<vShooterManager>().enabled = true;
            if (GetComponent<vAmmoManager>()) GetComponent<vAmmoManager>().enabled = true;
            if (GetComponent<vHeadTrack>()) GetComponent<vHeadTrack>().enabled = true;
            if (GetComponent<vLockOn>()) GetComponent<vLockOn>().enabled = true;

            if (GetComponent<MultiplayerMeleeManager>()) GetComponent<MultiplayerMeleeManager>().enabled = true;
            if (GetComponent<MultiplayerMeleeCombatInput>()) GetComponent<MultiplayerMeleeCombatInput>().enabled = true;
            if (GetComponent<MultiplayerGenericAction>()) GetComponent<MultiplayerGenericAction>().enabled = true;
        }
        //if (GetComponent<vGenericAction>()) GetComponent<vGenericAction>().enabled = true;
        //if (GetComponent<MultiplayerGenericAction>()) GetComponent<MultiplayerGenericAction>().enabled = true;
        //if (GetComponent<vMeleeManager>()) GetComponent<vMeleeManager>().enabled = false;
        //if (GetComponent<MultiplayerMeleeManager>()) GetComponent<MultiplayerMeleeManager>().enabled = true;
        //if (GetComponent<vMeleeCombatInput>()) GetComponent<vMeleeCombatInput>().enabled = false;
        //if (GetComponent<MultiplayerMeleeCombatInput>()) GetComponent<MultiplayerMeleeCombatInput>().enabled = true;

        VerifyBones();
    }
    void VerifyBones()
    {
        if (t_head == null)
        {
            try
            {
                t_head = _anim.GetBoneTransform(HumanBodyBones.Head).transform;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
            }
        }
        if (t_neck == null)
        {
            try
            {
                t_neck = _anim.GetBoneTransform(HumanBodyBones.Neck).transform;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
            }
        }
        if (t_spine == null)
        {
            try
            {
                t_spine = _anim.GetBoneTransform(HumanBodyBones.Spine).transform;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
            }
        }
        if (t_chest == null)
        {
            try
            {
                t_chest = _anim.GetBoneTransform(HumanBodyBones.Chest).transform;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
            }
        }
    }
    #endregion

    #region UpdateClient/ServerLogic
    void FixedUpdate()
    {
        if (isLocalPlayer == true)
        {
            if (GetComponent<vHeadTrack>())
            {
                TransmitBoneRotations();
            }
        }
    }
    void Update()
    {
        if (isLocalPlayer == true)
        {
            TransmitPlayerMovement();
        }
        else
        {
            SyncMovement();
        }
    }
    void LateUpdate()
    {
        if (isLocalPlayer == false)
        {
            if (GetComponent<vHeadTrack>())
            {
                SyncBoneRotation();
            }
        }
    }

    void SyncMovement()
    {
        transform.position = Vector3.Lerp(transform.position, player_position, Time.deltaTime * positionLerp);
        if (player_rotation != new Quaternion(0, 0, 0, 0))
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, player_rotation, Time.deltaTime * rotationLerp);
        }
    }
    void SyncBoneRotation()
    {
        t_head.localRotation = Quaternion.Lerp(t_head.localRotation, head, Time.deltaTime * lerpRate);
        t_neck.localRotation = Quaternion.Lerp(t_neck.localRotation, neck, Time.deltaTime * lerpRate);
        t_spine.localRotation = Quaternion.Lerp(t_spine.localRotation, spine, Time.deltaTime * lerpRate);
        t_chest.localRotation = Quaternion.Lerp(t_chest.localRotation, chest, Time.deltaTime * lerpRate);
    }
    #endregion

    #region NetworkSyncLogic
    [Client]
    void TransmitBoneRotations()
    {
        if (isLocalPlayer == true)
        {
            Cmd_RecieveRotations(t_head.localRotation, t_neck.localRotation, t_spine.localRotation, t_chest.localRotation);
        }
    }
    [Client]
    void TransmitPlayerMovement()
    {
        if (isLocalPlayer == true)
        {
            Cmd_RecievePlayerMovement(transform.position,transform.rotation);
        }
    }
    
    //Server update its variables
    [Command]
    void Cmd_RecieveRotations(Quaternion s_head, Quaternion s_neck, Quaternion s_spine, Quaternion s_chest)
    {
        head = s_head;
        neck = s_neck;
        spine = s_spine;
        chest = s_chest;
    }
    [Command]
    void Cmd_RecievePlayerMovement(Vector3 position, Quaternion rotation)
    {
        player_position = position;
        player_rotation = rotation;
    }
    #endregion
}
