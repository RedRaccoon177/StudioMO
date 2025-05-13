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
        MovePlayerModelPosition(player);
    }

    private void MovePlayerModelPosition(PlayerController player)
    {
        Vector3 camPos = player.HeadCameraPos.transform.position;
        player.PlayerModel.transform.position = new Vector3(camPos.x, camPos.y - 0.5f, camPos.z);

        Vector3 camEuler = player.HeadCameraPos.transform.eulerAngles;
        player.PlayerModel.transform.rotation = Quaternion.Euler(0, camEuler.y, 0);
    }

    public void CheckNowState(PlayerController player)
    {
        player.NowState = PlayerStateName.Move;
    }
}
