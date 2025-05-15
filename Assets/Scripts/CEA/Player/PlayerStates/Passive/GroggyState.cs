using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//행동불가 상태
public class GroggyState : IPlayerState
{
    MonoBehaviour _mono;
    PlayerController _player;

    public void EnterState(PlayerController player)
    {
        Debug.Log("그로기 실행됨");

        _player = player;

        //TODO
        //- 애니메이션 실행
        //- 사운드 실행
        //- 이팩트 실행

        //- 코루틴을 실행시켜서 30초 동안 참거짓으로 막아두기?
        _mono.StartCoroutine(StartAfterDelay());
    }

    public void UpdateState(PlayerController player) { }
    public void FixedUpdateState(PlayerController player) { }
    public void CheckNowState(PlayerController player)
    {
        player.NowState = PlayerStateName.Groggy;
    }

    IEnumerator StartAfterDelay()
    {
        yield return new WaitForSeconds(_player.groggyStateTime); // 30초 기다린다
        
        // 무적 상태로 전환
        _player.ChangeState(new InvincibleState());
    }
}
