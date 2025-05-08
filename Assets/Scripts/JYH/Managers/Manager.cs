using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 각각의 씬 안에 유일한 객체로 존재하며 씬 안에 포함되는 모든 객체들을 하향식으로 통제함
/// </summary>
[DisallowMultipleComponent]
public abstract class Manager : MonoBehaviour
{
    //씬 전환을 위한 별도의 컴포넌트가 필요함, 그 클래스가 있다면 그 클래스 객체를 멤버로 가지고 있어야 함(아마 로딩바를 포함한 ui이 패널이 될 듯함)

    private static Manager _instance = null;
#if UNITY_EDITOR

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
                SetupDefaultConfig();
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

    protected virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = FindObjectOfType<Manager>();
        }
        if (this == _instance)
        {
            SetupDefaultConfig();
        }
    }

    //객체나 규격을 기본값으로 설정해주는 함수
    protected abstract void SetupDefaultConfig();
}