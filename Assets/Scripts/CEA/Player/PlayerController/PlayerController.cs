using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Inputs;

//�÷��̾� ���� ��Ʈ�ѷ�
public partial class PlayerController : MonoBehaviour
{
    IPlayerState playerState;

    [Header("��� ��Ʈ�ڽ�")]
    public Collider pickaxeHitbox;

    [Header("VR ��ǲ �׼�")]
    [SerializeField]
    private InputActionManager actionManager;

    private InputActionAsset playerInput;
    private InputActionMap XRIRightHandInteration;
    private InputActionMap XRILeftHandLocomotion;

    private InputAction rightHandTrigger;
    private InputAction leftHandMove;

    private IPlayerState currentState;

    private void Awake()
    {
        playerInput = actionManager.actionAssets[0];
    }

    private void Start()
    {
        XRIRightHandInteration = playerInput.FindActionMap("XRI RightHand Interaction");
        XRILeftHandLocomotion = playerInput.FindActionMap("XRI LeftHand Locomotion");

        rightHandTrigger = XRIRightHandInteration.FindAction("Activate");
        leftHandMove = XRILeftHandLocomotion.FindAction("Move");

        rightHandTrigger.performed += OnActivate;
        rightHandTrigger.canceled += OnActivate;

        leftHandMove.performed += OnMove;
        leftHandMove.canceled += OnMove;

        ChangeState(new IdleState());  
    }

    public void ChangeState(IPlayerState newState)
    {
        currentState = newState;
        playerState.EnterState(this);
    }

    private void Update()
    {
        playerState.UpdateState(this);
    }

    private void FixedUpdate()
    {
        playerState.FixedUpdateState(this); 
    }
}
