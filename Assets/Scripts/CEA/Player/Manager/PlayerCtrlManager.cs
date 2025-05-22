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

    //TODO: �Ŀ� ��Ʈ��ũ ���� �� �� �÷��̾� ȯ�渶�� �ٸ��� �������� �����ؾ���
    [SerializeField]
    private GameObject localPlayer;

    [Header("VR ��ǲ �׼�")]
    [SerializeField]
    private InputActionManager actionManager;

    #region ��ǲ �׼� ����
    private InputActionAsset inputAsset;
    private InputActionMap rightHandInterActionMap;
    private InputActionMap leftHandInterActionMap;
    private InputActionMap leftHandlocomotionMap;

    [SerializeField]
    private InputAction rightSelectAction;
    [SerializeField]
    private InputAction leftSelectAction;
    [SerializeField]
    private InputAction moveAction;
    #endregion

    private Player localPlayerController;

    private void Awake()
    {
        photon = GetComponent<PhotonView>();
        inputAsset = actionManager.actionAssets[0];
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
        //if (context.performed)
        //{
        //    localPlayerController.OnRightSelect(true);
        //}

        //else if (context.canceled)
        //{
        //    localPlayerController.OnRightSelect(false);
        //}
    }

    private void OnLeftSelect(InputAction.CallbackContext context)
    {
        //if (context.performed)
        //{
        //    localPlayerController.OnLeftSelect(true);
        //}

        //else if (context.canceled)
        //{
        //    localPlayerController.OnLeftSelect(false);
        //}
    }

    public void Set(Player player)
    {
        localPlayer = player.gameObject;
        localPlayerController = player;

       // localPlayerController.SetXR(mainXROrigin.Camera, mainXROrigin);

        SetupInputActions();
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        //if (context.performed)
        //{
        //    Vector2 moveInput = context.ReadValue<Vector2>();
        //    localPlayerController.OnMove(moveInput);
        //}

        //else if (context.canceled)
        //{
        //    localPlayerController.OnMove(Vector2.zero);
        //}
    }
}
