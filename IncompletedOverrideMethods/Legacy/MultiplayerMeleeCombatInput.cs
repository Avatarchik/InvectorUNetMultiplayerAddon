using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Invector.vCharacterController;                    //To extend "vMeleeCombatInput"

public class MultiplayerMeleeCombatInput : vMeleeCombatInput
{
    #region Override Methods For Multiplayer Support
    protected override void MeleeWeakAttackInput()
    {
        if (isLocalPlayer == false) return;
        if (weakAttackInput.GetButtonDown() && MeleeAttackStaminaConditions())
        {
            TransmitAnimatorTrigger("WeakAttack");
            base.MeleeWeakAttackInput();
        }

    }
    protected override void MeleeStrongAttackInput()
    {
        if (isLocalPlayer == false) return;
        if (strongAttackInput.GetButtonDown() && MeleeAttackStaminaConditions())
        {
            TransmitAnimatorTrigger("StrongAttack");
            base.MeleeStrongAttackInput();
        }
    }

    protected override bool MeleeAttackConditions
    {
        get
        {
            if (meleeManager == null) meleeManager = GetComponent<MultiplayerMeleeManager>();
            return meleeManager != null && !cc.customAction && !cc.lockMovement && !cc.isCrouching;
        }
    }
    #endregion

    #region Network Sync Logic
    [ClientRpc]
    void Rpc_SetTrigger(string name)
    {
        if (isLocalPlayer == false)
        {
            this.GetComponent<Animator>().SetTrigger(name);
        }
    }
    [Client]
    void TransmitAnimatorTrigger(string triggername)
    {
        if (isLocalPlayer == true)
        {
            Cmd_Recieve_animTrigger(triggername);
        }
    }
    [Command]
    void Cmd_Recieve_animTrigger(string trigger_name)
    {
        Rpc_SetTrigger(trigger_name);
    }
    #endregion
}
