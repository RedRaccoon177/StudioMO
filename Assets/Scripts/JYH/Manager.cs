using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// ������ �� �ȿ� ������ ��ü�� �����ϸ� �� �ȿ� ���ԵǴ� ��� ��ü���� ��������� ������
/// </summary>
[DisallowMultipleComponent]
public abstract class Manager : MonoBehaviour
{
    //�� ��ȯ�� ���� ������ ������Ʈ�� �ʿ���, �� Ŭ������ �ִٸ� �� Ŭ���� ��ü�� ����� ������ �־�� ��(�Ƹ� �ε��ٸ� ������ ui�� �г��� �� ����)

#if UNITY_EDITOR
    private static Manager _instance = null;

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

    //
    public abstract void Initialize();

}