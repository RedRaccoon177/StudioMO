using System;
using UnityEngine;
using Photon.Pun;

[DisallowMultipleComponent]
[RequireComponent(typeof(PhotonView))]
[RequireComponent(typeof(Rigidbody))]
public partial class Player : MonoBehaviourPunCallbacks
{
    private bool _hasRigidbody = false;

    private Rigidbody _rigidbody = null;

    private Rigidbody getRigidbody {
        get
        {
            if(_hasRigidbody == false)
            {
                _rigidbody = GetComponent<Rigidbody>();
                _hasRigidbody = true;
            }
            return _rigidbody;
        }
    }

    private Vector3 _direction = Vector3.zero;

    [Header("�Ӹ�"), SerializeField]
    private Transform headTransform;

    [Header("�޼�"), SerializeField]
    private Transform leftHandTransform;

    [Header("������"), SerializeField]
    private Transform rightHandTransform;

    [Header("������ �ӵ�"), SerializeField, Range(0, int.MaxValue)]
    private float moveSpeed;

    private IPlayerState currentState;

    public PlayerStateName NowState;

    public bool MoveOn;

    public bool RightSelectOn;

    public bool LeftSelectOn;



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

    public void ChangeState(IPlayerState newState)
    {
        currentState = newState;
        currentState.EnterState(this);
        currentState.CheckNowState(this);
    }

    //private void Update()
    //{
    //    transform.position += _direction * Time.deltaTime * moveSpeed;
    //    //getRigidbody.velocity = _direction * Time.deltaTime * moveSpeed;
    //}

    //�÷��̾��� ���� ������ �޼���
    public void UpdateHead(Quaternion rotation)
    {
        if (photonView.IsMine == true && headTransform != null)
        {
            headTransform.rotation = rotation;
        }
    }

    //�÷��̾��� �޼��� �����̴� �޼���
    public void UpdateLeftHand(Vector3 position, Quaternion rotation)
    {
        if (photonView.IsMine == true)
        {
            leftHandTransform.Set(position, rotation);
        }
    }

    //�÷��̾��� �������� �����̴� �޼���
    public void UpdateRightHand(Vector3 position, Quaternion rotation)
    {
        if (photonView.IsMine == true)
        {
            rightHandTransform.Set(position, rotation);
        }
    }

    //�÷��̾��� �̵��� ����ϴ� �޼���
    public void UpdateMove(Vector2 input)
    {
        if (photonView.IsMine == true && headTransform != null)
        {
            _direction = headTransform.right * input.x + headTransform.forward * input.y;
            _direction.y = 0;
            transform.position += _direction * Time.deltaTime * moveSpeed;
        }
    }

    //������ ȹ���� ���� ���� ��������ִ� �Լ�
    public void AddMineral(uint value)
    {
        mineral += value;
        mineralReporter?.Invoke(mineral);
    }
}