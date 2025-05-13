using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Simulation;
using Photon.Pun;

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

    protected static Manager instance = null;      //�� �� �ȿ� �ܵ����� �����ϱ� ���� �̱��� ����

    private static readonly string LanguageTag = "Language";

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

    protected virtual void Update()
    {
        if (fixedPosition != null)
        {
            xrOrigin?.MoveCameraToWorldLocation(fixedPosition.Value);
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

    protected virtual void ChangeText(Translation.Language language)
    {
        Translation.Set(language);
    }

    //�� ���� �´� �ʱ�ȭ �Լ�
    protected abstract void Initialize();
}