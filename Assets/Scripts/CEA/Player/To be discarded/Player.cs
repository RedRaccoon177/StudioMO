using System;
using UnityEngine;
using Photon.Pun;

[DisallowMultipleComponent]
[RequireComponent(typeof(PhotonView))]
public class Player : MonoBehaviourPunCallbacks
{
    [Header("머리"), SerializeField]
    private Transform headTransform;
    [Header("왼손"), SerializeField]
    private Transform leftHandTransform;
    [Header("오른손"), SerializeField]
    private Transform rightHandTransform;

    private uint mineral = 0;   //채굴한 광물의 양

    public event Action<uint> mineralReporter;

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