using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;                               //for syncing across the network (multiplayer)
using Invector.vCharacterController.vActions;               //to extend "vGenericAction"

public class MultiplayerGenericAction : vGenericAction
{
    protected override void Start()
    {
        if (isLocalPlayer == false) return;
        base.Start();
    }
    protected override void TriggerAnimation()
    {
        if (isLocalPlayer == false) return;
        base.TriggerAnimation();
    }
    protected override void AnimationBehaviour()
    {
        if (isLocalPlayer == false) return;
        base.AnimationBehaviour();
    }
    protected override void ApplyPlayerSettings()
    {
        if (isLocalPlayer == false) return;
        base.ApplyPlayerSettings();
    }
    protected override void ResetPlayerSettings()
    {
        if (isLocalPlayer == false) return;
        base.ResetPlayerSettings();
    }
    protected override bool playingAnimation
    {
        get
        {
            if (isLocalPlayer == false)
            {
                return false;
            }
            else
            {
                return base.playingAnimation;
            }
        }
    }
}
