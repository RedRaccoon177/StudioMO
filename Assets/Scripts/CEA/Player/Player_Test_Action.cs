using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//��Ʈ�ѷ��� ���� �Է¹޴� ��, �Է� ���θ� �����ϴ� �޼��带 ��Ƴ��� ��ũ��Ʈ�Դϴ�.
[RequireComponent(typeof(PhotonView))]
public partial class Player_Test : MonoBehaviourPunCallbacks
{
    private bool moveOn;
    private bool rightSelectOn;
    private bool leftSelectOn;

    //���� ��Ʈ�ѷ� ���̽�ƽ (�̵�)
    public void OnMove(Vector2 input)
    {
        if (input != Vector2.zero)
        {
            MoveOn = true;
        }

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
