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

        localPlayerController.SetXR(mainXROrigin.Camera, mainXROrigin);

        SetupInputActions();
    }

    private void SetupInputActions()
    {
        rightHandInterActionMap = inputAsset.FindActionMap("XRI RightHand Interaction");
        leftHandInterActionMap = inputAsset.FindActionMap("XRI LeftHand Interaction");
        leftHandlocomotionMap = inputAsset.FindActionMap("XRI LeftHand Locomotion");

        rightSelectAction = rightHandInterActionMap.FindAction("Select");
        leftSelectAction = leftHandInterActionMap.FindAction("Select");
        moveAction = leftHandlocomotionMap.FindAction("Move");


        rightSelectAction.performed += OnRightSelect;
        rightSelectAction.canceled += OnRightSelect;

        leftSelectAction.performed += OnLeftSelect;
        leftSelectAction.canceled += OnLeftSelect;

        moveAction.performed += OnMove;
        moveAction.canceled += OnMove;
    }

    private void OnRightSelect(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            localPlayerController.OnRightSelect(true);
        }

        else if (context.canceled)
        {
            localPlayerController.OnRightSelect(false);
        }
    }

    private void OnLeftSelect(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            localPlayerController.OnLeftSelect(true);
        }

        else if (context.canceled)
        {
            localPlayerController.OnLeftSelect(false);
        }
    }

    public void Set(Player player)
    {

    }

    void Update()
    {
    private void OnMove(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Vector2 moveInput = context.ReadValue<Vector2>();
            localPlayerController.OnMove(moveInput);
        }

        else if (context.canceled)
        {
            localPlayerController.OnMove(Vector2.zero);
        }
    }
}
