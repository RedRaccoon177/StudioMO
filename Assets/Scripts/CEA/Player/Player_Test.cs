using UnityEngine;
using Photon.Pun;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

//TODO: 나중에 구현 다 끝나면 클래스명 Player_Test -> Player로 바꿔야함
[DisallowMultipleComponent]
[RequireComponent(typeof(PhotonView))]
public partial class Player_Test : MonoBehaviourPunCallbacks
{
    #region 필드
    [Header("플레이어 모델링")]
    [SerializeField]
    private GameObject playerModel;

    [Header("머리"), SerializeField]
    private Transform headTransform;

    [Header("왼손"), SerializeField]
    private Transform leftHandTransform;

    [Header("오른손"), SerializeField]
    private Transform rightHandTransform;

    [Header("카메라 위치")]
    private Camera cameraPos;

    [Space(10)][SerializeField]
    private DynamicMoveProvider moveProvider;

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
    #endregion

    public void Start()
    {
        moveSpeed = moveProvider.moveSpeed;

        cameraPos = Camera.main;

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
            Vector3 camEuler = cameraPos.transform.eulerAngles;
            Quaternion targetRotation = Quaternion.Euler(0, camEuler.y, 0);
            headTransform.rotation = targetRotation;
        }
    }

    public void FixedUpdate()
    {
        if(photonView.IsMine)
        {
            currentState.FixedUpdateState(this);

            if (MoveOn)
            {
                playerMove();
            }
        }
    }

    public void playerMove() //플레이어 움직임
    {
        Vector3 camForward = cameraPos.transform.forward;
        Vector3 camRight = cameraPos.transform.right;

        camForward.y = 0;
        camRight.y = 0;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 moveDirection = camForward * moveInput.y + camRight * moveInput.x;
        transform.position += moveDirection * moveSpeed * Time.fixedDeltaTime;
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


}