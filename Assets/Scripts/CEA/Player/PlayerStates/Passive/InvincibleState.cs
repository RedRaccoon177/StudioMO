using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//무적 상태
public class InvincibleState : IPlayerState
{
    public void EnterState(PlayerController player)
    {

    }

    public void FixedUpdateState(PlayerController player)
    {

    }

    public void UpdateState(PlayerController player)
    {

    }

    public void CheckNowState(PlayerController player)
    {
        player.NowState = PlayerStateName.Invincible;
    }
}
