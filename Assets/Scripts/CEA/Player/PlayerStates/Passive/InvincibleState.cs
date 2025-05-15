using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���� ����
public class InvincibleState : IPlayerState
{
    MonoBehaviour _mono;
    PlayerController _player;
    Coroutine _delayCoroutine;

    public void EnterState(PlayerController player)
    {
        _player = player;
        _mono = player;

        //TODO
        //- �ִϸ��̼� ����
        //- ���� ����
        //- ����Ʈ ����

        //- �ڷ�ƾ�� ������Ѽ� 30�� ���� ���������� ���Ƶα�?
        _delayCoroutine = _mono.StartCoroutine(StartAfterDelay());
    }

    public void FixedUpdateState(PlayerController player) { }
    public void UpdateState(PlayerController player) { }
    public void CheckNowState(PlayerController player)
    {
        player.NowState = PlayerStateName.Invincible;
    }

    IEnumerator StartAfterDelay()
    {
        yield return new WaitForSeconds(_player.invincibleStateTime);

        _player.isGroggyAndinvincibleState = false;
        _player.ChangeState(new IdleState());
    }
}
