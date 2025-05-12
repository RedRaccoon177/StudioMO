using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Inputs;

//�÷��̾� ���� ��Ʈ�ѷ�
public partial class PlayerController : MonoBehaviour
{
    [Header("��� ��Ʈ�ڽ�")]
    public Collider pickaxeHitbox;

    [Header("VR ��ǲ �׼�")]
    [SerializeField]
    private InputActionManager actionManager;

    [Header("�÷��̾� ���� ī�޶� ��ġ")]
    [SerializeField]
    private Transform headCameraPos;

    [Header("�÷��̾� �𵨸� ��ġ")]
    [SerializeField]
    private Transform playerModelPos;

    private InputActionAsset playerInput;
    private InputActionMap XRIRightHandInteration;
    private InputActionMap XRILeftHandLocomotion;
    private InputActionMap XRILeftHandInteraction;

    private InputAction rightHandTrigger;
    private InputAction leftHandMove;
    private InputAction leftHandTrigger;

    private IPlayerState currentState;

    private PlayerStateName nowState;

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
        currentState.EnterState(this);
        currentState.CheckNowState(this);
    }

    private void Update()
    {
        currentState.UpdateState(this);
        //Debug.Log(nowState);
    }

    private void FixedUpdate()
    {
        currentState.FixedUpdateState(this);

        if (moveOn == true)
        {
            MovePlayerModelPosition();
        }
    }

    private void MovePlayerModelPosition()
    {
        Vector3 headCamPos = headCameraPos.position;
        playerModelPos.position = new Vector3(headCamPos.x, playerModelPos.position.y, headCamPos.z);
    }
}
