using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//상호작용 상태
public class InteractState : IPlayerState
{
    public void EnterState(Player_Test player)
    {
        //player.pickaxeHitbox.SetActive(true);
    }

    public void UpdateState(Player_Test player)
    {
        if(player.RightSelectOn == false)
        {
            player.ChangeState(new IdleState());
        }
    }

    public void FixedUpdateState(Player_Test player)
    {

    }

    public void CheckNowState(Player_Test player)
    {
        player.NowState = PlayerStateName.Interact;
    }

}
