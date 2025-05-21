using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���� ����
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
        //- �ִϸ��̼� ����
        //- ���� ����
        //- ����Ʈ ����

        //- �ڷ�ƾ�� ������Ѽ� 30�� ���� ���������� ���Ƶα�?
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
