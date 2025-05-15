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

    public DynamicMoveProvider moveProvider;

    public float moveSpeed;

    private Vector2 moveInput;

    private IPlayerState currentState;

    private PlayerStateName nowState;

    #region 플레이어에게 필요한 필드들
    [Header("행동불능 상태 시간")]
    public float groggyStateTime = 30f;

    [Header("무적 상태 시간")]
    public float invincibleStateTime = 3f;

    [Header("넉백 상태 시간")]
    public float knockbackStateTime = 1f;

    [Header("넉백 면역 상태 시간")]
    public float solidStateTime = 1f;
    #endregion

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

        leftHandMove.performed += OnMove;
        leftHandMove.canceled += OnMove;

        moveSpeed = moveProvider.moveSpeed;


        //TODO: new IdleState()같이 new들 GC생각해서 추후 다 재사용으로 전환하기
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

        if (Input.GetKeyDown(KeyCode.Q))
        {
            HitBullet();
        }
    }

    private void FixedUpdate()
    {
        currentState.FixedUpdateState(this);

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

    /// <summary>
    /// 탄막 접촉 시 플레이어 행동불능 상태 함수
    /// </summary>
    void HitBullet()
    {
        ChangeState(new GroggyState());
    }
}
