using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//������Ƽ ���� ��ũ��Ʈ

//�⺻������ playercontroller�� ������ �� �չ��ڸ�
//�빮�ڷ� ��ȯ�� ���·� �����մϴ�.
//��κ��� �������� private�� ����Ǿ� �ֽ��ϴ�. ������Ƽ�� ���� �������ּ���.

public partial class PlayerController : MonoBehaviour
{
    public Camera HeadCameraPos
    {
        get { return headCameraPos; }
        set { headCameraPos = value; }
    }

    public GameObject PlayerModel
    {
        get { return playerModel; }
        set { playerModel = value; }
    }

    public Collider PlayerHitBox
    {
        get { return playerHitBox; }
        set { playerHitBox = value; }   
    }


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
