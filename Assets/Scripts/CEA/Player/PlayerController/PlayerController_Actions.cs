using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//��ǲ �Ŵ����� �޴� �÷��̾� �׼ǵ� ����
public partial class PlayerController : MonoBehaviour
{
    private bool moveOn;
    private bool rightSelectOn;
    private bool leftSelectOn;

    //���� ��Ʈ�ѷ� ���̽�ƽ (�̵�)
    void OnMove(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            moveOn = true;
            moveInput = context.ReadValue<Vector2>();
        }
        
        else if(context.canceled)
        {
            moveOn= false;
        }
    }

    //������ ��Ʈ�ѷ� �׸� ��ư (select) 
    void OnRightSelect(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            rightSelectOn = true;
        }

        else if (context.canceled)
        {
            rightSelectOn = false;
            pickaxeHitbox.gameObject.SetActive(false);
        }
    }

    //���� ��Ʈ�ѷ� �׸� ��ư (select)
    void OnLeftSelect(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            leftSelectOn = true;
        }

        else if (context.canceled)
        {
            leftSelectOn = false;
        }
    }
}
