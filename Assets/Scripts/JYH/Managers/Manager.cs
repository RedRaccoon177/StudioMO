using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Simulation;

/// <summary>
/// ������ �� �ȿ� ������ ��ü�� �����ϸ� �� �ȿ� ���ԵǴ� ��� ��ü���� ��������� ������
/// </summary>
[DisallowMultipleComponent]
public abstract class Manager : MonoBehaviour
{
    [Header(nameof(Manager))]
    [SerializeField]
    private XRDeviceSimulator deviceSimulator;      //XR ����̽� �ùķ����͸� ����ϱ� ���� ����
    [SerializeField]
    private XROrigin xrOrigin;                      //XR �������� ����ϱ� ���� ����
    protected Vector3? fixedPosition;               //��ġ ������ �ϱ� ���� ����

    //�� ��ȯ ����� ž���� ������ Ŭ������ ����� �ʿ���(�Ƹ� �ε��ٸ� ������ ui�� �г��� �� ����)

    private static Manager _instance = null;        //�� �� �ȿ� �ܵ����� �����ϱ� ���� �̱��� ����

#if UNITY_EDITOR

    [SerializeField]
    private bool deviceSimulatorEnabled = true;    //����̽� �ùķ����͸� ����ϱ� ���� ����

    protected virtual void OnValidate()
    {
        if (gameObject.scene == SceneManager.GetActiveScene())
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<Manager>();
            }
            if (this == _instance)
            {
                name = GetType().Name;
                if (Application.isPlaying == false)
                {
                    SetupDefaultConfig();
                }
            }
            UnityEditor.EditorApplication.delayCall += () =>
            {
                if (this != _instance && this != null)
                {
                    UnityEditor.Undo.DestroyObjectImmediate(gameObject);
                }
            };
        }
    }

#endif

    private void Start()
    {
        if (_instance == null)
        {
            _instance = FindObjectOfType<Manager>();
        }
        if (this == _instance)
        {
            SetupDefaultConfig();
            Initialize();
            if (deviceSimulator != null)
            {
#if UNITY_EDITOR
                if (deviceSimulator.deviceSimulatorUI != null)
                {
                    deviceSimulator.deviceSimulatorUI.SetActive(!deviceSimulatorEnabled);
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

    //�� ���� �´� �ʱ�ȭ �Լ�
    protected abstract void Initialize();

    //��ü�� �԰��� �⺻������ �������ִ� �Լ�
    protected abstract void SetupDefaultConfig();
}