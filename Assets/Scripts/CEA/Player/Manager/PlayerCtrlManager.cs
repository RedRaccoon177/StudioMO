using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using Photon.Pun;
using UnityEngine.XR.Interaction.Toolkit.Inputs;

public class PlayerCtrlManager : MonoBehaviour
{
    public XROrigin mainXROrigin;

    //TODO: 후에 네트워크 연결 시 각 플레이어 환경마다 다르게 꽂히도록 변경해야함
    [SerializeField]
    private GameObject localPlayer;

    [Header("VR 인풋 액션")]
    [SerializeField]
    private InputActionManager actionManager;


    void Start()
    {
        
    }


    void Update()
    {
        
    }
}
