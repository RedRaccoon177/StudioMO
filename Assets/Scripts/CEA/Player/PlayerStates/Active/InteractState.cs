using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//��ȣ�ۿ� ����
public class InteractState : IPlayerState
{
    public void EnterState(PlayerController player)
    {
        player.pickaxeHitbox.gameObject.SetActive(true);
    }

    public void UpdateState(PlayerController player)
    {
        if(player.ActivateOn == false)
        {
            player.ChangeState(new IdleState());
        }
    }

    public void FixedUpdateState(PlayerController player)
    {

    }

    public void CheckNowState(PlayerController player)
    {
        player.NowState = PlayerStateName.Interact;
    }

}
