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
