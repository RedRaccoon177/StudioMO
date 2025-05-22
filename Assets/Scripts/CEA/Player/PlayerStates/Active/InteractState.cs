using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//상호작용 상태
public class InteractState : IPlayerState
{
    public void EnterState(Player player)
    {
        //player.pickaxeHitbox.SetActive(true);
    }

    public void UpdateState(Player player)
    {
        if(player.RightSelectOn == false)
        {
            player.ChangeState(new IdleState());
        }

    }

    public void FixedUpdateState(Player player)
    {

    }

    public void CheckNowState(Player player)
    {
        player.NowState = PlayerStateName.Interact;
    }

}
