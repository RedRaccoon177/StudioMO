using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�˹� �鿪 ����
public class SolidState : IPlayerState
{
    public void EnterState(Player_Test player)
    {

    }

    public void FixedUpdateState(Player_Test player)
    {

    }

    public void UpdateState(Player_Test player)
    {

    }

    public void CheckNowState(Player_Test player)
    {
        player.NowState = PlayerStateName.Solid;
    }
}
