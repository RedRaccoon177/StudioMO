using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class TestPhoton : MonoBehaviourPunCallbacks
{
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) == true)
        {
            PhotonNetwork.LeaveRoom();
            SceneManager.LoadScene("JYH");
        }
    }

    public void Initialize()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        PhotonNetwork.JoinRandomOrCreateRoom();
    }

    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.IsMasterClient == true)
        {
            GameObject gameObject = PhotonNetwork.InstantiateRoomObject("TestObject", Vector3.zero, Quaternion.identity);
            gameObject.GetComponent<TestObject>().Initialize();
        }
    }
}
