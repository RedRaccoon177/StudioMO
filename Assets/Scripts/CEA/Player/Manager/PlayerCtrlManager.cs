using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using Photon.Pun;
using UnityEngine.XR.Interaction.Toolkit.Inputs;
using UnityEngine.InputSystem;

public class PlayerCtrlManager : MonoBehaviour
{
    public XROrigin mainXROrigin;

    private PhotonView photon;

    //TODO: 후에 네트워크 연결 시 각 플레이어 환경마다 다르게 꽂히도록 변경해야함
    [SerializeField]
    private GameObject localPlayer;

    [Header("VR 인풋 액션")]
    [SerializeField]
    private InputActionManager actionManager;

    #region 인풋 액션 관련
    private InputActionAsset inputAsset;
    private InputActionMap rightHandInterActionMap;
    private InputActionMap leftHandInterActionMap;
    private InputActionMap leftHandlocomotionMap;

    private InputAction rightSelectAction;
    private InputAction leftSelectAction;
    private InputAction moveAction;
    #endregion

    private Player_Test localPlayerController;

    private void Awake()
    {
        photon = GetComponent<PhotonView>();
        localPlayerController = localPlayer.GetComponent<Player_Test>();
        inputAsset = actionManager.actionAssets[0];
    }


    private void Start()
    {
        if (!photon.IsMine)
        {
            return;
        }

        localPlayerController.SetXR(mainXROrigin.Camera,mainXROrigin);

        Camera localCamera = GetComponentInChildren<Camera>();

        rightHandInterActionMap = inputAsset.FindActionMap("XRI RightHand Interaction");
        leftHandInterActionMap = inputAsset.FindActionMap("XRI LeftHand Interaction");
        leftHandlocomotionMap = inputAsset.FindActionMap("XRI LeftHand Locomotion");

        rightSelectAction = rightHandInterActionMap.FindAction("Select");
        leftSelectAction = leftHandInterActionMap.FindAction("Select");
        moveAction = leftHandlocomotionMap.FindAction("Move");

        rightSelectAction.performed += ctx => localPlayerController?.OnRightSelect(true);
        rightSelectAction.canceled += ctx => localPlayerController?.OnRightSelect(false);

        leftSelectAction.performed += ctx => localPlayerController?.OnLeftSelect(true);
        leftSelectAction.canceled += ctx => localPlayerController?.OnLeftSelect(false);

        moveAction.performed += ctx => localPlayerController?.OnMove(ctx.ReadValue<Vector2>());
        moveAction.canceled += ctx => localPlayerController?.OnMove(Vector2.zero);

        rightSelectAction.Enable();
        leftSelectAction.Enable();
        moveAction.Enable();

        localPlayerController = GetComponentInChildren<Player_Test>();
    }


    public void Set(Player player)
    {

    }

    void Update()
    {

    }
}
