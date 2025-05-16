using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//평상 시 상태
public class IdleState : IPlayerState
{
    public void EnterState(PlayerController player)
    {

    }

    public void UpdateState(PlayerController player)
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

        
    }

    public void FixedUpdateState(PlayerController player)
    {

    }

    public void CheckNowState(PlayerController player)
    {
        player.NowState = PlayerStateName.Idle;
    }
}
