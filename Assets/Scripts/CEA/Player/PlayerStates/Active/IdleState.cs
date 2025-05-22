using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//평상 시 상태
public class IdleState : IPlayerState
{
    public void EnterState(Player_Test player)
    {

    }

    public void UpdateState(Player_Test player)
    {
        if(player.MoveOn == true)
        {
            player.ChangeState(new MoveState());
            return;
        }

        if(player.RightSelectOn == true)
        {
            player.ChangeState(new InteractState());
            return;
        }

        if (player.LeftSelectOn == true)
        {
            player.ChangeState(new SlowMotionState());
            return;
        }
        
    }

    public void FixedUpdateState(Player_Test player)
    {

    }

    public void CheckNowState(Player_Test player)
    {
        player.NowState = PlayerStateName.Idle;
    }
}
