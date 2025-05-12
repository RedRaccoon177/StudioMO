using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : IPlayerState
{
    public void EnterState(PlayerController player)
    {

    }

    public void UpdateState(PlayerController player)
    {
        if (player.MoveOn == false)
        {
            player.ChangeState(new IdleState());
            return;
        }

        if(player.ActivateOn == true)
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
        player.NowState = PlayerStateName.Move;
    }
}
