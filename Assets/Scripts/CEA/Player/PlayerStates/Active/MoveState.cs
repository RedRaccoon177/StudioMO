using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : IPlayerState
{
    public void EnterState(Player_Test player)
    {

    }

    public void UpdateState(Player_Test player)
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

    public void FixedUpdateState(Player_Test player)
    {
        //MovePlayerModelPosition(player);
    } 

    private void MovePlayerModelPosition(Player_Test player)
    {
        #region move
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
        #endregion
    }

    public void CheckNowState(Player_Test player)
    {
        player.NowState = PlayerStateName.Move;
    }
}
