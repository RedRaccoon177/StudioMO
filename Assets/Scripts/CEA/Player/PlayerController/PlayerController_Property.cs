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

    public Vector2 MoveInput
    {
        get { return moveInput; }
        set { moveInput = value; }
    }

    public bool MoveOn
    {
        get { return moveOn; }
        set { moveOn = value; }
    }


    public bool RightSelectOn
    {
        get { return rightSelectOn; }
        set { rightSelectOn = value; }
    }

    public bool LeftSelectOn
    {
        get { return leftSelectOn; }
        set { leftSelectOn = value; }
    }

    public PlayerStateName NowState
    {
        get { return nowState; }
        set { nowState = value; }
    }

}
