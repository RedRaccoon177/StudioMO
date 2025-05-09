using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

[DisallowMultipleComponent]
[RequireComponent(typeof(PhotonView))]
[RequireComponent(typeof(PhotonTransformView))]
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

    //public override void OnPlayerEnteredRoom(Player player)
    //{

    //}
}
