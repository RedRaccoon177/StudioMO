using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�ൿ�Ұ� ����
public class GroggyState : IPlayerState
{
    MonoBehaviour _mono;
    Player_Test _player;

    public void EnterState(Player_Test player)
    {
        _player = player;

        //TODO
        //- �ִϸ��̼� ����
        //- ���� ����
        //- ����Ʈ ����

        //- �ڷ�ƾ�� ������Ѽ� 30�� ���� ���������� ���Ƶα�?
        _mono.StartCoroutine(StartAfterDelay());
    }

    public void UpdateState(Player_Test player) { }
    public void FixedUpdateState(Player_Test player) { }
    public void CheckNowState(Player_Test player)
    {
        player.NowState = PlayerStateName.Groggy;
    }

    IEnumerator StartAfterDelay()
    {
        yield return new WaitForSeconds(_player.groggyStateTime); // 30�� ��ٸ���
        
        // ���� ���·� ��ȯ
        _player.ChangeState(new InvincibleState());
    }
}
