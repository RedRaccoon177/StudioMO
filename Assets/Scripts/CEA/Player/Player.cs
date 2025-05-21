using UnityEngine;
using Photon.Pun;

[DisallowMultipleComponent]
[RequireComponent(typeof(PhotonView))]
public class Player : MonoBehaviourPunCallbacks
{
    [Header("�Ӹ�"), SerializeField]
    private Transform headTransform;
    [Header("�޼�"), SerializeField]
    private Transform leftHandTransform;
    [Header("������"), SerializeField]
    private Transform rightHandTransform;

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