using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�˹� �鿪 ����
public class SolidState : IPlayerState
{
    public void EnterState(PlayerController player)
    {

    }

    public void FixedUpdateState(PlayerController player)
    {

    }

    public void UpdateState(PlayerController player)
    {

    }

    public void CheckNowState(PlayerController player)
    {
        player.NowState = PlayerStateName.Solid;
    }
}
