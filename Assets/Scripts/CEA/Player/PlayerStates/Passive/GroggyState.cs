using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//행동불가 상태
public class GroggyState : IPlayerState
{
    MonoBehaviour _mono;
    Player _player;
    Coroutine _delayCoroutine;

    public void EnterState(Player player)
    {
        Debug.Log("그로기 실행됨");

        _player = player;
        _mono = player;

        //TODO
        //- 애니메이션 실행
        //- 사운드 실행
        //- 이팩트 실행

        _delayCoroutine = _mono.StartCoroutine(StartAfterDelay());
    }

    public void UpdateState(Player player) { }
    public void FixedUpdateState(Player player) { }
    public void CheckNowState(Player player)
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
