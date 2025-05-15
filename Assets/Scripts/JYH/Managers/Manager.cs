using System;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Simulation;
using Photon.Pun;
using UnityEngine.InputSystem;

/// <summary>
/// 각각의 씬 안에 유일한 객체로 존재하며 씬 안에 포함되는 모든 객체들을 하향식으로 통제함
/// </summary>
[DisallowMultipleComponent]
public abstract class Manager : MonoBehaviourPunCallbacks
{
    [Header(nameof(Manager))]
    [SerializeField]
    private XRDeviceSimulator deviceSimulator;      //XR 디바이스 시뮬레이터를 사용하기 위한 변수
    [SerializeField]
    private XROrigin xrOrigin;                      //XR 오리진을 사용하기 위한 변수
    protected Vector3? fixedPosition;               //위치 고정을 하기 위한 변수

    protected static Manager instance = null;       //각 씬 안에 단독으로 존재하기 위한 싱글톤 변수

    private static readonly string LanguageTag = "Language";

    private Stack<Action> actionStack = new Stack<Action>();

    [SerializeField]
    private InputActionReference primaryInputAction;    //XR 디바이스 시뮬레이터의 Primary Input을 사용하기 위한 변수
    protected event Action primaryAction;               //Primary Input이 눌렸을 때 발생하는 이벤트
    [SerializeField]
    private InputActionReference secondaryInputAction;  //XR 디바이스 시뮬레이터의 Secondary Input을 사용하기 위한 변수
    protected event Action secondaryAction;             //Secondary Input이 눌렸을 때 발생하는 이벤트

#if UNITY_EDITOR

    [SerializeField]
    private bool deviceSimulatorEnabled = true;    //디바이스 시뮬레이터를 사용하기 위한 변수

    [Header("언어 변경"), SerializeField]
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

    //각 씬에 맞는 초기화 함수
    protected abstract void Initialize();
}