using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//컨트롤러를 통해 입력받는 값, 입력 여부를 매핑하는 메서드를 모아놓은 스크립트입니다.
[RequireComponent(typeof(PhotonView))]
public partial class Player_Test : MonoBehaviourPunCallbacks
{
    private bool moveOn;
    private bool rightSelectOn;
    private bool leftSelectOn;

    //왼쪽 컨트롤러 조이스틱 (이동)
    public void OnMove(Vector2 input)
    {
        if (input != Vector2.zero)
        {
            MoveOn = true;
        }

        moveInput = input;
    }

    //오른쪽 컨트롤러 그립 버튼 (select) 
    public void OnRightSelect(bool isPressed)
    {
        rightSelectOn = isPressed;

        if (!isPressed)
        {
            //pickaxeHitbox.SetActive(false);
        }
    }

    //왼쪽 컨트롤러 그립 버튼 (select)
    public void OnLeftSelect(bool isPressed)
    {
        leftSelectOn = isPressed;
    }
}
