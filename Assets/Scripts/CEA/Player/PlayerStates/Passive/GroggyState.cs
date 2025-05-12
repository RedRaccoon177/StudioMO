using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//행동불가 상태
public class GroggyState : IPlayerState
{
    public void EnterState(PlayerController player)
    {

    }

    public void UpdateState(PlayerController player)
    {

    }

    public void FixedUpdateState(PlayerController player)
    {

    }

    public void CheckNowState(PlayerController player)
    {
        player.NowState = PlayerStateName.Groggy;
    }
}
