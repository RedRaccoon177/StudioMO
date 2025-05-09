using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Inputs;

public class PlayerController : MonoBehaviour
{
    IPlayerState playerState;

    [Header("��� ��Ʈ�ڽ�")]
    public Collider pickaxeHitbox;

    [Header("VR ��ǲ �׼�")]
    [SerializeField]
    private InputActionManager actionManager;

    InputActionAsset playerInput;
    InputActionMap xrInputActionMap;

    InputAction rightHandInteraction;

    private void Awake()
    {
        playerInput = actionManager.actionAssets[0];
    }

    private void Start()
    {
        xrInputActionMap = playerInput.FindActionMap("XRI RightHand Interaction");

        rightHandInteraction = xrInputActionMap.FindAction("Activate");
    }


    private void Update()
    {
        //playerState.UpdateState(this);
    }

    private void FixedUpdate()
    {
        //playerState.FixedUpdateState(this); 
    }
}
