using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�ൿ�Ұ� ����
public class GroggyState : IPlayerState
{
    MonoBehaviour _mono;
    PlayerController _player;

    public void EnterState(PlayerController player)
    {
        Debug.Log("�׷α� �����");

        _player = player;

        //TODO
        //- �ִϸ��̼� ����
        //- ���� ����
        //- ����Ʈ ����

        //- �ڷ�ƾ�� ������Ѽ� 30�� ���� ���������� ���Ƶα�?
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
        yield return new WaitForSeconds(_player.groggyStateTime); // 30�� ��ٸ���
        
        // ���� ���·� ��ȯ
        _player.ChangeState(new InvincibleState());
    }
}
