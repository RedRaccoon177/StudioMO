using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Simulation;

/// <summary>
/// 각각의 씬 안에 유일한 객체로 존재하며 씬 안에 포함되는 모든 객체들을 하향식으로 통제함
/// </summary>
[DisallowMultipleComponent]
public abstract class Manager : MonoBehaviour
{
    [Header(nameof(Manager))]
    [SerializeField]
    private XRDeviceSimulator deviceSimulator;      //XR 디바이스 시뮬레이터를 사용하기 위한 변수
    [SerializeField]
    private XROrigin xrOrigin;                      //XR 오리진을 사용하기 위한 변수
    protected Vector3? fixedPosition;               //위치 고정을 하기 위한 변수

    //씬 전환 기능을 탑재한 별도의 클래스가 멤버로 필요함(아마 로딩바를 포함한 ui이 패널이 될 듯함)

    private static Manager _instance = null;        //각 씬 안에 단독으로 존재하기 위한 싱글톤 변수

#if UNITY_EDITOR

    [SerializeField]
    private bool deviceSimulatorEnabled = true;    //디바이스 시뮬레이터를 사용하기 위한 변수

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

    //각 씬에 맞는 초기화 함수
    protected abstract void Initialize();

    //객체나 규격을 기본값으로 설정해주는 함수
    protected abstract void SetupDefaultConfig();
}