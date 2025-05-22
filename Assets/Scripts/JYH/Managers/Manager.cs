using System;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Simulation;
using Photon.Pun;
using TMPro;

/// <summary>
/// ������ �� �ȿ� ������ ��ü�� �����ϸ� �� �ȿ� ���ԵǴ� ��� ��ü���� ��������� ������
/// </summary>
[DisallowMultipleComponent]
public abstract class Manager : MonoBehaviourPunCallbacks
{
    protected static Manager instance = null;       //�� �� �ȿ� �ܵ����� �����ϱ� ���� �̱��� ����

    protected static readonly Vector3 CameraOffsetPosition = new Vector3(0, 1.36144f, 0);

    [Header(nameof(Manager))]
    [SerializeField]
    private XROrigin xrOrigin;                      //XR �������� ����ϱ� ���� ����
    protected Vector3? fixedPosition;               //��ġ ������ �ϱ� ���� ����

    private static readonly string LanguageTag = "Language";

    private Stack<Action> actionStack = new Stack<Action>();

    [SerializeField]
    private InputActionReference primaryInputAction;    //XR ����̽� �ùķ������� Primary Input�� ����ϱ� ���� ����
    protected event Action primaryAction;               //Primary Input�� ������ �� �߻��ϴ� �̺�Ʈ
    [SerializeField]
    private InputActionReference secondaryInputAction;  //XR ����̽� �ùķ������� Secondary Input�� ����ϱ� ���� ����
    protected event Action secondaryAction;             //Secondary Input�� ������ �� �߻��ϴ� �̺�Ʈ

    [SerializeField]
    private TMP_FontAsset[] tmpFontAssets = new TMP_FontAsset[Translation.count];

    protected TMP_FontAsset tmpFontAsset
    {
        get;
        private set;
    }

    [SerializeField]
    private XRDeviceSimulator deviceSimulator;      //XR ����̽� �ùķ����͸� ����ϱ� ���� ����

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
                ExtensionMethod.Sort(ref tmpFontAssets, Translation.count);
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
            Initialize();
            ChangeText((Translation.Language)PlayerPrefs.GetInt(LanguageTag));
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

    private void ChangeText(Translation.Language language)
    {
        Translation.Set(language);
        switch (language)
        {
            case Translation.Language.English:
            case Translation.Language.Korean:
            case Translation.Language.Chinese:
            case Translation.Language.Japanese:
                tmpFontAsset = tmpFontAssets[(int)language];
                break;
        }
        ChangeText();
    }

    protected virtual void Update()
    {
        if (fixedPosition != null)
        {
            xrOrigin?.MoveCameraToWorldLocation(fixedPosition.Value);
        }
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

    //Ư�� �ൿ�� �����ϰ� ������ �߰� �ϰų� ���� �ൿ�� �����ϴ� �Լ�
    protected void Refresh(Action action)
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

    //�� �ٲ��ִ� �Լ�
    protected void SetLanguage(int index)
    {
        if (index >= 0 && index < Translation.count)
        {
            PlayerPrefs.SetInt(LanguageTag, index);
            ChangeText((Translation.Language)index);
        }
    }

    //�� ���� �´� �ʱ�ȭ �Լ�
    protected abstract void Initialize();

    //�� �ٲ��ִ� �Լ�
    protected abstract void ChangeText();
}