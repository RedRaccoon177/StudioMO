using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�ൿ�Ұ� ����
public class GroggyState : IPlayerState
{
    MonoBehaviour _mono;
    PlayerController _player;
    Coroutine _delayCoroutine;

    public void EnterState(PlayerController player)
    {
        Debug.Log("�׷α� �����");

        _player = player;
        _mono = player;

        //TODO
        //- �ִϸ��̼� ����
        //- ���� ����
        //- ����Ʈ ����

        _delayCoroutine = _mono.StartCoroutine(StartAfterDelay());
    }

    public void UpdateState(PlayerController player) { }
    public void FixedUpdateState(PlayerController player) { }
    public void CheckNowState(PlayerController player)
    {
        player.NowState = PlayerStateName.Groggy;
    }

    IEnumerator StartAfterDelay()
    {
        _player.isGroggyAndinvincibleState = true;

        yield return new WaitForSeconds(_player.groggyStateTime);
        
        // ���� ���·� ��ȯ
        _player.ChangeState(new InvincibleState());
    }
}
