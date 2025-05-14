using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//��ǲ �Ŵ����� �޴� �÷��̾� �׼ǵ� ����
public partial class PlayerController : MonoBehaviour
{
    private bool moveOn;
    private bool activateOn;

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

    //������ ��Ʈ�ѷ� Ʈ���� 
    void OnRightSelect(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            activateOn = true;
        }

        else if (context.canceled)
        {
            activateOn = false;
            pickaxeHitbox.gameObject.SetActive(false);
        }
    }
}
