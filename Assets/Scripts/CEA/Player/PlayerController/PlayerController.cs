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

    [Space (10)][SerializeField]
    private PickaxeController playerPickaxe;

    [Space(10)][SerializeField]
    private DynamicMoveProvider moveProvider;

    private InputActionAsset playerInput;
    private InputActionMap XRIRightHandInteration;
    private InputActionMap XRILeftHandLocomotion;
    private InputActionMap XRILeftHandInteraction;

    private InputAction rightHandGrip;

    private InputAction leftHandMove;
    private InputAction leftHandGrip;
    private InputAction leftHandTrigger;


    private float moveSpeed;

    private Vector2 moveInput;

    public IPlayerState currentState;

    private PlayerStateName nowState;

    #region �÷��̾�� �ʿ��� �ʵ��
    [Header("�ൿ�Ҵ� ���� �ð�")]
    public float groggyStateTime = 30f;
    public bool isGroggyAndinvincibleState = false;

    [Header("���� ���� �ð�")]
    public float invincibleStateTime = 3f;

    [Header("�˹� ���� �ð�")]
    public float knockbackStateTime = 1f;

    [Header("�˹� �鿪 ���� �ð�")]
    public float solidStateTime = 1f;
    #endregion

    private void Awake()
    {
        //TODO: ���� ��Ʈ��ũ ���� ��ų �� �����ؾ���.
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
            transform.position += moveDirection * moveSpeed * Time.fixedDeltaTime;
        }

        Vector3 camEuler = headCameraPos.transform.eulerAngles;
         Quaternion targetRotation = Quaternion.Euler(0, camEuler.y, 0);
         playerModel.transform.rotation = targetRotation;

        //xr device simulator�� �̵� �ڵ�

        //Vector3 camPos = headCameraPos.transform.position;
        //playerModel.transform.position = new Vector3(camPos.x, camPos.y, camPos.z);
        //
        //Vector3 camEuler = headCameraPos.transform.eulerAngles;
        //playerModel.transform.rotation = Quaternion.Euler(0, camEuler.y, 0);
    }

    /// <summary>
    /// ź�� ���� �� �÷��̾� �ൿ�Ҵ� ���� �Լ�
    /// </summary>
    public void HitBullet()
    {
        ChangeState(new GroggyState());
    }
}
