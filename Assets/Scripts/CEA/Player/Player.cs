using UnityEngine;
using Photon.Pun;

[DisallowMultipleComponent]
[RequireComponent(typeof(PhotonView))]
public class Player : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Transform leftHandTransform;

    [SerializeField]
    private Transform rightHandTransform;

    public void UpdateLeftHand(Vector3 position, Quaternion rotation)
    {
        if (photonView.IsMine == true)
        {
            leftHandTransform.Set(position, rotation);
        }
    }

    public void UpdateRightHand(Vector3 position, Quaternion rotation)
    {
        if (photonView.IsMine == true)
        {
            rightHandTransform.Set(position, rotation);
        }
    }
}
