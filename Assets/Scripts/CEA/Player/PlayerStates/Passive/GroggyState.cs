using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�ൿ�Ұ� ����
public class GroggyState : IPlayerState
{
    MonoBehaviour _mono;
    Player_Test _player;
    Coroutine _delayCoroutine;

    public void EnterState(Player_Test player)
    {
        Debug.Log("�׷α� �����");

        _player = player;
        _mono = player;

        //TODO
        //- �ִϸ��̼� ����
        //- ���� ����
        //- ����Ʈ ����

        //- ������ ���� ��Ű��

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
        
        // ���� ���·� ��ȯ
        _player.ChangeState(new InvincibleState());
    }
}
