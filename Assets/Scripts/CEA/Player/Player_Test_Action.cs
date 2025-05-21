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

    //���� ��Ʈ�ѷ� ���̽�ƽ (�̵�)
    public void OnMove(Vector2 input)
    {
        moveOn = input != Vector2.zero;
        moveInput = input;
    }

    //������ ��Ʈ�ѷ� �׸� ��ư (select) 
    public void OnRightSelect(bool isPressed)
    {
        rightSelectOn = isPressed;
        if (!isPressed)
        {
            //pickaxeHitbox.SetActive(false);
        }
    }

    //���� ��Ʈ�ѷ� �׸� ��ư (select)
    public void OnLeftSelect(bool isPressed)
    {
        leftSelectOn = isPressed;
    }
}
