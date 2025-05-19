using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//프로퍼티 모음 스크립트

//기본적으로 playercontroller의 변수명 맨 앞문자를
//대문자로 변환한 형태로 통일합니다.
//대부분의 변수들은 private로 선언되어 있습니다. 프로퍼티를 통해 접근해주세요.

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
