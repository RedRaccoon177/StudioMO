using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//특수능력 상태
public class SlowMotionState : IPlayerState
{
    public void EnterState(PlayerController player)
    {
        SlowMotionOn();
    }

    public void FixedUpdateState(PlayerController player)
    {

    }

    public void UpdateState(PlayerController player)
    {
        if(player.LeftSelectOn == false)
        {
            SlowMotionOff();
            player.ChangeState(new IdleState());
        }
    }

    private void SlowMotionOn()
    {
        Time.timeScale = 0.5f;
    }

    private void SlowMotionOff()
    {
        Time.timeScale = 1.0f;
    }

    public void CheckNowState(PlayerController player)
    {
        player.NowState = PlayerStateName.SlowMotion;
    }
}
