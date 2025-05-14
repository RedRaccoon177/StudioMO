using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Inputs;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

//플레이어 메인 컨트롤러
public partial class PlayerController : MonoBehaviour
{
    [Header("곡괭이 히트박스")]
    public Collider pickaxeHitbox;

    [Header("VR 인풋 액션")]
    [SerializeField]
    private InputActionManager actionManager;

    [Header("플레이어 시점 카메라 위치")]
    [SerializeField]
    private Camera headCameraPos;

    [Header("플레이어 모델링")]
    [SerializeField]
    private GameObject playerModel;

    [Header("플레이어 피격 범위")]
    [SerializeField]
    private Collider playerHitBox;

    private InputActionAsset playerInput;
    private InputActionMap XRIRightHandInteration;
    private InputActionMap XRILeftHandLocomotion;
    private InputActionMap XRILeftHandInteraction;

    private InputAction rightHandGrip;

    private InputAction leftHandMove;
    private InputAction leftHandGrip;
    private InputAction leftHandTrigger;

    [SerializeField]
    private DynamicMoveProvider moveProvider;

    private float moveSpeed;

    private Vector2 moveInput;

    private IPlayerState currentState;

    private PlayerStateName nowState;

    private void Awake()
    {
        playerInput = actionManager.actionAssets[0];
    }

    private void Start()
    {
        XRIRightHandInteration = playerInput.FindActionMap("XRI RightHand Interaction");
        XRILeftHandInteraction = playerInput.FindActionMap("XRI LeftHand Interaction");
        XRILeftHandLocomotion = playerInput.FindActionMap("XRI LeftHand Locomotion");

        rightHandGrip = XRIRightHandInteration.FindAction("Select");
        leftHandGrip = XRILeftHandInteraction.FindAction("Select");
        leftHandMove = XRILeftHandLocomotion.FindAction("Move");

        rightHandGrip.performed += OnRightSelect;
        rightHandGrip.canceled += OnRightSelect;

        leftHandGrip.performed += OnLeftSelect;
        leftHandGrip.canceled += OnLeftSelect;

        leftHandMove.performed += OnMove;
        leftHandMove.canceled += OnMove;

        moveSpeed = moveProvider.moveSpeed;

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
        Debug.Log(nowState);
    }

    private void FixedUpdate()
    {
        currentState.FixedUpdateState(this);

        if(moveOn == true)
        {
            Vector3 camForward = headCameraPos.transform.forward;
            Vector3 camRight = headCameraPos.transform.right;

            camForward.y = 0;
            camRight.y = 0;
            camForward.Normalize();
            camRight.Normalize();

            Vector3 moveDirection = camForward * moveInput.y + camRight * moveInput.x;

            playerModel.transform.position += moveDirection * moveSpeed * Time.fixedDeltaTime;
        }

        Vector3 camEuler = headCameraPos.transform.eulerAngles;
        Quaternion targetRotation = Quaternion.Euler(0, camEuler.y, 0);
        playerModel.transform.rotation = targetRotation;

        //xr device simulator용 이동 코드

        //Vector3 camPos = headCameraPos.transform.position;
        //playerModel.transform.position = new Vector3(camPos.x, camPos.y - 0.5f, camPos.z);
        //
        //Vector3 camEuler = headCameraPos.transform.eulerAngles;
        //playerModel.transform.rotation = Quaternion.Euler(0, camEuler.y, 0);
    }
}
