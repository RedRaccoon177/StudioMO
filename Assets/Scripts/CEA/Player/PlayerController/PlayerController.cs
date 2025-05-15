using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Inputs;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

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
    private Camera headCameraPos;

    [Header("�÷��̾� �𵨸�")]
    [SerializeField]
    private GameObject playerModel;

    [Header("�÷��̾� �ǰ� ����")]
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

    #region �÷��̾�� �ʿ��� �ʵ��
    [Header("�ൿ�Ҵ� ���� �ð�")]
    public float groggyStateTime = 30f;

    [Header("���� ���� �ð�")]
    public float invincibleStateTime = 3f;

    [Header("�˹� ���� �ð�")]
    public float knockbackStateTime = 1f;

    [Header("�˹� �鿪 ���� �ð�")]
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


        //TODO: new IdleState()���� new�� GC�����ؼ� ���� �� �������� ��ȯ�ϱ�
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

        //xr device simulator�� �̵� �ڵ�

        //Vector3 camPos = headCameraPos.transform.position;
        //playerModel.transform.position = new Vector3(camPos.x, camPos.y - 0.5f, camPos.z);
        //
        //Vector3 camEuler = headCameraPos.transform.eulerAngles;
        //playerModel.transform.rotation = Quaternion.Euler(0, camEuler.y, 0);

    }

    /// <summary>
    /// ź�� ���� �� �÷��̾� �ൿ�Ҵ� ���� �Լ�
    /// </summary>
    void HitBullet()
    {
        ChangeState(new GroggyState());
    }
}
