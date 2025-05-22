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
    [Header("플레이어 모델링")]
    [SerializeField]
    private GameObject playerModel;

    [Header("머리"), SerializeField]
    private Transform headTransform;

    [Header("왼손"), SerializeField]
    private Transform leftHandTransform;

    [Header("왼손 컨트롤러"), SerializeField]
    private GameObject leftController;

    [Header("오른손"), SerializeField]
    private Transform rightHandTransform;

    [Header("오른손 컨트롤러"), SerializeField]
    private GameObject rightController;

    private Camera playerCamera;

    private XROrigin mainXROrigin;

    [Space(10)]
    [SerializeField]
    private DynamicMoveProvider moveProvider;

    [Header("움직임 속도"), SerializeField]
    private float moveSpeed;

    private Vector2 moveInput;

    private IPlayerState currentState;

    private PlayerStateName nowState;


    [Header("행동불능 상태 시간")]
    public float groggyStateTime = 30f;

    [Header("무적 상태 시간")]
    public float invincibleStateTime = 3f;

    [Header("넉백 상태 시간")]
    public float knockbackStateTime = 1f;

    [Header("넉백 면역 상태 시간")]
    public float solidStateTime = 1f;

    private uint mineral = 0;   //채굴한 광물의 양

    public event Action<uint> mineralReporter;

    private void Awake()
    {
        PlayerCtrlManager manager = FindObjectOfType<PlayerCtrlManager>();
        manager.Set(this);
    }

    private void Start()
    {
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

    public void playerMove() //플레이어 움직임
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

    //플레이어가 고개를 돌릴 때 호출되는 함수 
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

    //플레이어가 왼손을 움직일 때 호출되는 함수
    public void UpdateLeftHand(Vector3 position, Quaternion rotation)
    {
        if (photonView.IsMine == true)
        {
            leftHandTransform.Set(position, rotation);
        }
    }

    //플레이어가 오른손을 움직일 때 호출되는 함수
    public void UpdateRightHand(Vector3 position, Quaternion rotation)
    {
        if (photonView.IsMine == true)
        {
            rightHandTransform.Set(position, rotation);
        }
    }


    //광물을 획득한 현재 양을 적용시켜주는 함수
    public void AddMineral(uint value)
    {
        mineral += value;
        mineralReporter?.Invoke(mineral);
    }
}