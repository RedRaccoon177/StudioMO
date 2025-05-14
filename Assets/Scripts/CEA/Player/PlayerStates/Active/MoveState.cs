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

        if(player.RightSelectOn == true)
        {
            player.ChangeState(new InteractState());
            return;
        }
    }

    public void FixedUpdateState(PlayerController player)
    {
        MovePlayerModelPosition(player);
    } 

    private void MovePlayerModelPosition(PlayerController player)
    {
        //Vector3 camForward = player.HeadCameraPos.transform.forward;
        //Vector3 camRight = player.HeadCameraPos.transform.right;

        //camForward.y = 0;
        //camRight.y = 0;
        //camForward.Normalize();
        //camRight.Normalize();

        //Vector3 moveDirection = camForward * player.MoveInput.y + camRight * player.MoveInput.x;

        //player.PlayerModel.transform.position += moveDirection * player.moveSpeed * Time.fixedDeltaTime;

        //Vector3 camEuler = player.HeadCameraPos.transform.eulerAngles;
        //Quaternion targetRotation = Quaternion.Euler(0, camEuler.y, 0);
        //player.PlayerModel.transform.rotation = targetRotation; 
    }

    public void CheckNowState(PlayerController player)
    {
        player.NowState = PlayerStateName.Move;
    }
}
