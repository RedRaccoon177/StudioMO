using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//프로퍼티 모음 스크립트
//기본적으로 playercontroller의 변수명 맨 앞문자를
//대문자로 변환한 형태로 통일합니다.
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
