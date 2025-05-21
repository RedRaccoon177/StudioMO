using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PhotonView))]
public partial class Player_Test : MonoBehaviourPunCallbacks
{
    private bool moveOn;
    private bool rightSelectOn;
    private bool leftSelectOn;

    //왼쪽 컨트롤러 조이스틱 (이동)
    public void OnMove(Vector2 input)
    {
        moveOn = input != Vector2.zero;
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
