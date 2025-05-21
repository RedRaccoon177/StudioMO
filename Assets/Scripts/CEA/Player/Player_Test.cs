using UnityEngine;
using Photon.Pun;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;
using Unity.XR.CoreUtils;

//caution : _Test(����� �׽�Ʈ) ��� ���� ��ũ��Ʈ�� �÷��̾� ������Դϴ�. ���� �����丵 �����Դϴ�.
//TODO: ���߿� ���� �� ������ Ŭ������ Player_Test -> Player�� �ٲ����
[DisallowMultipleComponent]
[RequireComponent(typeof(PhotonView))]
public partial class Player_Test : MonoBehaviourPunCallbacks
{
    #region �ʵ�
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

    [Space(10)][SerializeField]
    private DynamicMoveProvider moveProvider;

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
    #endregion

    public void Start()
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

            rightHandTransform.Set(rightController.transform.position,rightController.transform.rotation);
            leftHandTransform.Set(leftController.transform.position, leftController.transform.rotation);
        }
    }

    public void FixedUpdate()
    {
        if(photonView.IsMine)
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
        Vector3 originForward = mainXROrigin.transform.forward;
        Vector3 originRight = mainXROrigin.transform.right;

        originForward.y = 0;
        originRight.y = 0;
        originForward.Normalize();
        originRight.Normalize();

        Vector3 moveDirection = originForward * moveInput.y + originRight * moveInput.x;
        transform.position += moveDirection * moveSpeed * Time.fixedDeltaTime;
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
}