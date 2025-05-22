using System;
using UnityEngine;
using Photon.Pun;
using Unity.XR.CoreUtils;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;
using ExitGames.Client.Photon.StructWrapping;

[DisallowMultipleComponent]
[RequireComponent(typeof(PhotonView))]
public partial class Player : MonoBehaviourPunCallbacks
{
    [Header("�÷��̾� �𵨸�")]
    [SerializeField]
    private GameObject playerModel;

    [Header("�Ӹ�"), SerializeField]
    private Transform headTransform;

    [Header("�޼�"), SerializeField]
    private Transform leftHandTransform;

    [Header("�޼� ��Ʈ�ѷ�"), SerializeField]
    private GameObject leftController;

    [Header("������"), SerializeField]
    private Transform rightHandTransform;

    [Header("������ ��Ʈ�ѷ�"), SerializeField]
    private GameObject rightController;

    private Camera playerCamera;

    private XROrigin mainXROrigin;

    [Space(10)]
    [SerializeField]
    private DynamicMoveProvider moveProvider;

    [Header("������ �ӵ�"), SerializeField]
    private float moveSpeed;

    private Vector2 moveInput;

    private IPlayerState currentState;

    private PlayerStateName nowState;


    [Header("�ൿ�Ҵ� ���� �ð�")]
    public float groggyStateTime = 30f;

    [Header("���� ���� �ð�")]
    public float invincibleStateTime = 3f;

    [Header("�˹� ���� �ð�")]
    public float knockbackStateTime = 1f;

    [Header("�˹� �鿪 ���� �ð�")]
    public float solidStateTime = 1f;

    private uint mineral = 0;   //ä���� ������ ��

    public event Action<uint> mineralReporter;

    private void Awake()
    {
        PlayerCtrlManager manager = FindObjectOfType<PlayerCtrlManager>();
        manager.Set(this);
    }

    private void Start()
    {
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

    public void Update()
    {
        if (photonView.IsMine)
        {
            Debug.Log(nowState);

            currentState.UpdateState(this);
            currentState.UpdateState(this);

            if (playerCamera != null)
            {
                Vector3 camEuler = playerCamera.transform.eulerAngles;
                Quaternion targetRotation = Quaternion.Euler(0, camEuler.y, 0);
                headTransform.rotation = targetRotation;
            }

            rightHandTransform.Set(rightController.transform.position, rightController.transform.rotation);
            leftHandTransform.Set(leftController.transform.position, leftController.transform.rotation);
        }
    }

    public void FixedUpdate()
    {
        if (photonView.IsMine)
        {
            currentState.FixedUpdateState(this);

            Debug.Log(MoveOn);

            if (MoveOn)
            {
                playerMove();
            }

        }
    }

    public void playerMove() //�÷��̾� ������
    {
        Vector3 cameraForward = playerCamera.transform.forward;
        Vector3 cameraRight = playerCamera.transform.right;

        cameraForward.y = 0;
        cameraRight.y = 0;

        cameraForward.Normalize();
        cameraRight.Normalize();

        Vector3 moveDirection = cameraForward * moveInput.y + cameraRight * moveInput.x;

        Vector3 moveDelta = moveDirection * moveSpeed * Time.deltaTime;

        mainXROrigin.transform.position += moveDelta;
        playerModel.transform.position = mainXROrigin.transform.position;
    }

    public void SetXR(Camera xrCamera, XROrigin xrOrigin)
    {
        playerCamera = xrCamera;
        mainXROrigin = xrOrigin;
    }

    //�÷��̾ ���� ���� �� ȣ��Ǵ� �Լ� 
    public void UpdateMove(Vector3 position, Quaternion rotation)
    {
        if (photonView.IsMine == true)
        {
            transform.position = new Vector3(position.x, transform.position.y, position.z);
            if (headTransform != null)
            {
                headTransform.rotation = rotation;
            }
        }
    }

    //�÷��̾ �޼��� ������ �� ȣ��Ǵ� �Լ�
    public void UpdateLeftHand(Vector3 position, Quaternion rotation)
    {
        if (photonView.IsMine == true)
        {
            leftHandTransform.Set(position, rotation);
        }
    }

    //�÷��̾ �������� ������ �� ȣ��Ǵ� �Լ�
    public void UpdateRightHand(Vector3 position, Quaternion rotation)
    {
        if (photonView.IsMine == true)
        {
            rightHandTransform.Set(position, rotation);
        }
    }


    //������ ȹ���� ���� ���� ��������ִ� �Լ�
    public void AddMineral(uint value)
    {
        mineral += value;
        mineralReporter?.Invoke(mineral);
    }
}