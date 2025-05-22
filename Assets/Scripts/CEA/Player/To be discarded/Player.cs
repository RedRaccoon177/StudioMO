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

    [Header("�̵� �ӵ�"), SerializeField, Range(1, int.MaxValue)]
    private float moveSpeed = 10;
    [Header("�ൿ�Ҵ� ���� �ð�"), Range(0, int.MaxValue)]
    private float faintingTime = 30f;
    [Header("���� ���� �ð�"), Range(0, int.MaxValue)]
    private float invincibleTime = 3f;

    private uint mineral = 0;   //ä���� ������ ��

    public event Action<uint> mineralReporter;

    public float knockbackStateTime;
    public float groggyStateTime;
    public float invincibleStateTime = 3f;
    public PlayerStateName NowState;
    public bool MoveOn;
    public bool RightSelectOn;
    public bool LeftSelectOn;

    public void ChangeState(IPlayerState newState)
    {
    }

    private void FixedUpdate()
    {
        Vector3 position = getRigidbody.position + _direction.normalized * moveSpeed * Time.fixedDeltaTime;
        getRigidbody.MovePosition(position);
    }

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
        if (photonView.IsMine == true && leftHandTransform)
        {
            leftHandTransform.SetPositionAndRotation(position, rotation);
        }
    }

    //�÷��̾��� �������� �����̴� �޼���
    public void UpdateRightHand(Vector3 position, Quaternion rotation)
    {
        if (photonView.IsMine == true && rightHandTransform != null)
        {
            rightHandTransform.SetPositionAndRotation(position, rotation);
        }
    }

    //�÷��̾��� �̵��� ����ϴ� �޼���
    public void UpdateMove(Vector2 input)
    {
        if (photonView.IsMine == true && headTransform != null)
        {
            _direction = headTransform.right * input.x + headTransform.forward * input.y;
            _direction.y = 0;
        }
    }

    //������ ȹ���� ���� ���� ��������ִ� �Լ�
    public void AddMineral(uint value)
    {
        mineral += value;
        mineralReporter?.Invoke(mineral);
    }

    //
    public void Hit()
    {

    }
}