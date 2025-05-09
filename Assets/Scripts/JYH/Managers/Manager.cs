using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 각각의 씬 안에 유일한 객체로 존재하며 씬 안에 포함되는 모든 객체들을 하향식으로 통제함
/// </summary>
[DisallowMultipleComponent]
public abstract class Manager : MonoBehaviour
{
    //씬 전환 기능을 탑재한 별도의 클래스가 멤버로 필요함(아마 로딩바를 포함한 ui이 패널이 될 듯함)

    private static Manager _instance = null;
#if UNITY_EDITOR

    protected virtual void OnValidate()
    {
        if (gameObject.scene == SceneManager.GetActiveScene())
        {
            Initialize();
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

    protected virtual void Awake()
    {
        Initialize();
    }

    //초기화 함수
    private void Initialize()
    {
        if (_instance == null)
        {
            _instance = FindObjectOfType<Manager>();
        }
        if (this == _instance)
        {
            name = GetType().Name;
            SetupDefaultConfig();
        }
    }

    //객체나 규격을 기본값으로 설정해주는 함수
    protected abstract void SetupDefaultConfig();
}