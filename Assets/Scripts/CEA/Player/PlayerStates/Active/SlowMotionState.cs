using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//특수능력 상태
public class SlowMotionState : IPlayerState
{
    public void EnterState(Player player)
    {
        SlowMotionOn();
    }

    public void FixedUpdateState(Player player)
    {

    }

    public void UpdateState(Player player)
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

    public void CheckNowState(Player player)
    {
        player.NowState = PlayerStateName.SlowMotion;
    }
}
