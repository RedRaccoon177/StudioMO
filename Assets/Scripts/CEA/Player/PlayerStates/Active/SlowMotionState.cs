using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Ư���ɷ� ����
public class SlowMotionState : IPlayerState
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

    private void SlowMotion()
    {

    }

    public void CheckNowState(PlayerController player)
    {
        player.NowState = PlayerStateName.SlowMotion;
    }
}
