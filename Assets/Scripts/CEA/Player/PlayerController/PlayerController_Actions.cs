using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//인풋 매니저로 받는 플레이어 액션들 모음
public partial class PlayerController : MonoBehaviour
{
    private bool moveOn;
    private bool activateOn;

    //왼쪽 컨트롤러 조이스틱 (이동)
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

    //오른쪽 컨트롤러 트리거 
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
