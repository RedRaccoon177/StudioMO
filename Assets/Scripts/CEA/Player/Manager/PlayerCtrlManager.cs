using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using Photon.Pun;
using UnityEngine.XR.Interaction.Toolkit.Inputs;

public class PlayerCtrlManager : MonoBehaviour
{
    public XROrigin mainXROrigin;

    //TODO: �Ŀ� ��Ʈ��ũ ���� �� �� �÷��̾� ȯ�渶�� �ٸ��� �������� �����ؾ���
    [SerializeField]
    private GameObject localPlayer;

    [Header("VR ��ǲ �׼�")]
    [SerializeField]
    private InputActionManager actionManager;


    void Start()
    {
        
    }


    void Update()
    {
        
    }
}
