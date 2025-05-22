using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//넉백 상태
public class KnockBackState : IPlayerState
{
    MonoBehaviour _mono;
    Player_Test _player;

    public void EnterState(Player_Test player)
    {
        _player = player;
        _mono = player;

        //TODO
        //- 애니메이션 실행
        //- 사운드 실행
        //- 이팩트 실행

        //- 코루틴을 실행시켜서 30초 동안 참거짓으로 막아두기?
        _mono.StartCoroutine(StartAfterDelay());
    }

    public void FixedUpdateState(Player_Test player) { }

    public void UpdateState(Player_Test player) { }

    public void CheckNowState(Player_Test player)
    {
        player.NowState = PlayerStateName.Knockback;
    }

    IEnumerator StartAfterDelay()
    {
        yield return new WaitForSeconds(_player.knockbackStateTime);

        _player.ChangeState(new SolidState());
    }
}

