using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//무적 상태
public class InvincibleState : IPlayerState
{
    MonoBehaviour _mono;
    Coroutine _delayCoroutine;
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
        _delayCoroutine = _mono.StartCoroutine(StartAfterDelay());
    }

    public void FixedUpdateState(Player_Test player) { }
    public void UpdateState(Player_Test player) { }
    public void CheckNowState(Player_Test player)
    {
        player.NowState = PlayerStateName.Invincible;
    }

    IEnumerator StartAfterDelay()
    {
        yield return new WaitForSeconds(_player.invincibleStateTime);

        //_player.isGroggyAndinvincibleState = false;
        _player.ChangeState(new IdleState());
    }
}
