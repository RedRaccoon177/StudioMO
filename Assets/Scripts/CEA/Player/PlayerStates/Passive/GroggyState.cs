using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//행동불가 상태
public class GroggyState : IPlayerState
{
    MonoBehaviour _mono;
    Player_Test _player;
    Coroutine _delayCoroutine;

    public void EnterState(Player_Test player)
    {
        Debug.Log("그로기 실행됨");

        _player = player;
        _mono = player;

        //TODO
        //- 애니메이션 실행
        //- 사운드 실행
        //- 이팩트 실행

        //- 움직임 정지 시키기

        _delayCoroutine = _mono.StartCoroutine(StartAfterDelay());
    }

    public void UpdateState(Player_Test player) { }
    public void FixedUpdateState(Player_Test player) { }
    public void CheckNowState(Player_Test player)
    {
        player.NowState = PlayerStateName.Groggy;
    }

    IEnumerator StartAfterDelay()
    {
        //_player.isGroggyAndinvincibleState = true;

        yield return new WaitForSeconds(_player.groggyStateTime);
        
        // 무적 상태로 전환
        _player.ChangeState(new InvincibleState());
    }
}
