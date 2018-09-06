using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;                         //to add multiplayer support
using Invector.vMelee;                                //to extend "vMeleeManager"

public class MultiplayerMeleeManager : vMeleeManager
{
    protected override void Start()
    {
        if (isLocalPlayer == false) return;
        base.Start();
    }
    public override float GetAttackStaminaRecoveryDelay()
    {
        if (isLocalPlayer == false)
        {
            return 0.0f;
        }
        else
        {
            return base.GetAttackStaminaRecoveryDelay();
        }
    }
}
