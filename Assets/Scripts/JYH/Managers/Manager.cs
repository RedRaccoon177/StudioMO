using System;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Simulation;
using Photon.Pun;
using UnityEngine.InputSystem;

/// <summary>
/// ������ �� �ȿ� ������ ��ü�� �����ϸ� �� �ȿ� ���ԵǴ� ��� ��ü���� ��������� ������
/// </summary>
[DisallowMultipleComponent]
public abstract class Manager : MonoBehaviourPunCallbacks
{
    [Header(nameof(Manager))]
    [SerializeField]
    private XRDeviceSimulator deviceSimulator;      //XR ����̽� �ùķ����͸� ����ϱ� ���� ����
    [SerializeField]
    private XROrigin xrOrigin;                      //XR �������� ����ϱ� ���� ����
    protected Vector3? fixedPosition;               //��ġ ������ �ϱ� ���� ����

    protected static Manager instance = null;       //�� �� �ȿ� �ܵ����� �����ϱ� ���� �̱��� ����

    private static readonly string LanguageTag = "Language";

    private Stack<Action> actionStack = new Stack<Action>();

    [SerializeField]
    private InputActionReference primaryInputAction;    //XR ����̽� �ùķ������� Primary Input�� ����ϱ� ���� ����
    protected event Action primaryAction;               //Primary Input�� ������ �� �߻��ϴ� �̺�Ʈ
    [SerializeField]
    private InputActionReference secondaryInputAction;  //XR ����̽� �ùķ������� Secondary Input�� ����ϱ� ���� ����
    protected event Action secondaryAction;             //Secondary Input�� ������ �� �߻��ϴ� �̺�Ʈ

#if UNITY_EDITOR

    [SerializeField]
    private bool deviceSimulatorEnabled = true;    //����̽� �ùķ����͸� ����ϱ� ���� ����

    [Header("��� ����"), SerializeField]
    private Translation.Language language = Translation.Language.Korean;

    protected virtual void OnValidate()
    {
        if (gameObject.scene == SceneManager.GetActiveScene())
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Manager>();
            }
            if (this == instance)
            {
                name = GetType().Name;
                ChangeText(language);
            }
            UnityEditor.EditorApplication.delayCall += () =>
            {
                if (this != instance && this != null)
                {
                    UnityEditor.Undo.DestroyObjectImmediate(gameObject);
                }
            };
        }
    }

#endif

    private void Start()
    {
        if (instance == null)
        {
            instance = FindObjectOfType<Manager>();
        }
        if (this == instance)
        {
            ChangeText((Translation.Language)PlayerPrefs.GetInt(LanguageTag));
            Initialize();
            if (deviceSimulator != null)
            {
#if UNITY_EDITOR
                int childCount = deviceSimulator.transform.childCount;
                for (int i = 0; i < childCount; i++)
                {
                    deviceSimulator.transform.GetChild(i).gameObject.SetActive(deviceSimulatorEnabled);
                }
#else
                Destroy(deviceSimulator.gameObject);
#endif
            }
        }
    }

    private void OnPrimaryInputPressed(InputAction.CallbackContext callbackContext)
    {
        primaryAction?.Invoke();
    }

    private void OnSecondaryInputPressed(InputAction.CallbackContext callbackContext)
    {
        secondaryAction?.Invoke();
    }

    protected virtual void Update()
    {
        if (fixedPosition != null)
        {
            xrOrigin?.MoveCameraToWorldLocation(fixedPosition.Value);
        }
    }

    protected virtual void ChangeText(Translation.Language language)
    {
        Translation.Set(language);
    }

    public override void OnEnable()
    {
        base.OnEnable();
        primaryInputAction.Set(true, OnPrimaryInputPressed);
        secondaryInputAction.Set(true, OnSecondaryInputPressed);
    }

    public override void OnDisable()
    {
        base.OnDisable();
        primaryInputAction.Set(false, OnPrimaryInputPressed);
        secondaryInputAction.Set(false, OnSecondaryInputPressed);
    }

    protected void PlayAction(Action action)
    {
        if (action != null)
        {
            actionStack.Push(action);
            action.Invoke();
        }
        else if (actionStack.Count > 0)
        {
            actionStack.Pop().Invoke();
        }
    }

    public void SetLanguage(int index)
    {
        if (index >= byte.MinValue && index <= byte.MaxValue)
        {
            PlayerPrefs.SetInt(LanguageTag, index);
            ChangeText((Translation.Language)index);
        }
    }

    //�� ���� �´� �ʱ�ȭ �Լ�
    protected abstract void Initialize();
}