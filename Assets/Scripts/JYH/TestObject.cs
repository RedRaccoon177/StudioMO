using UnityEngine;
using Photon.Pun;

[DisallowMultipleComponent]
[RequireComponent(typeof(PhotonView))]
[RequireComponent(typeof(PhotonTransformView))]
[RequireComponent(typeof(PhotonAnimatorView))]
[RequireComponent(typeof(PhotonRigidbodyView))]
public class TestObject : MonoBehaviourPunCallbacks
{
    [PunRPC]
    private void SetActive(bool value)
    {
        gameObject.SetActive(value);
    }

    public void Initialize()
    {
        SetActive(false);
        if (PhotonNetwork.InRoom == true)
        {
            photonView.RPC("SetActive", RpcTarget.OthersBuffered, false);
        }
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player player)
    {

    }
}
