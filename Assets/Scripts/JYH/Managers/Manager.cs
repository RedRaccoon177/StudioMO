using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// ������ �� �ȿ� ������ ��ü�� �����ϸ� �� �ȿ� ���ԵǴ� ��� ��ü���� ��������� ������
/// </summary>
[DisallowMultipleComponent]
public abstract class Manager : MonoBehaviour
{
    //�� ��ȯ ����� ž���� ������ Ŭ������ ����� �ʿ���(�Ƹ� �ε��ٸ� ������ ui�� �г��� �� ����)

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

    //�ʱ�ȭ �Լ�
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

    //��ü�� �԰��� �⺻������ �������ִ� �Լ�
    protected abstract void SetupDefaultConfig();
}