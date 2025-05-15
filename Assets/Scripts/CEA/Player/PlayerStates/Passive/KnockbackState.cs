using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�˹� ����
public class KnockBackState : IPlayerState
{
    MonoBehaviour _mono;
    PlayerController _player;

    public void EnterState(PlayerController player)
    {
        _player = player;

        //TODO
        //- �ִϸ��̼� ����
        //- ���� ����
        //- ����Ʈ ����

        //- �ڷ�ƾ�� ������Ѽ� 30�� ���� ���������� ���Ƶα�?
        _mono.StartCoroutine(StartAfterDelay());
    }

    public void FixedUpdateState(PlayerController player) { }

    public void UpdateState(PlayerController player) { }

    public void CheckNowState(PlayerController player)
    {
        player.NowState = PlayerStateName.Knockback;
    }

    IEnumerator StartAfterDelay()
    {
        yield return new WaitForSeconds(_player.knockbackStateTime);

        _player.ChangeState(new SolidState());
    }
}

