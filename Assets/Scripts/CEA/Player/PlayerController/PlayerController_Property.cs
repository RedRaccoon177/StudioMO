using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//������Ƽ ���� ��ũ��Ʈ
//�⺻������ playercontroller�� ������ �� �չ��ڸ�
//�빮�ڷ� ��ȯ�� ���·� �����մϴ�.
public partial class PlayerController : MonoBehaviour
{
    public bool MoveOn
    {
        get { return moveOn; }
        set { moveOn = value; }
    }

    public bool ActivateOn
    {
        get { return activateOn; }
        set { activateOn = value; }
    }

    public PlayerStateName NowState
    {
        get { return nowState; }
        set { nowState = value; }
    }
}
