using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�ൿ�Ұ� ����
public class GroggyState : IPlayerState
{
    MonoBehaviour _mono;
    Player _player;
    Coroutine _delayCoroutine;

    public void EnterState(Player player)
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
        
        // ���� ���·� ��ȯ
        _player.ChangeState(new InvincibleState());
    }
}
