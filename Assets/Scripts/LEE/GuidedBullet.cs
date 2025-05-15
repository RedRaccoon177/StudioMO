using UnityEngine;
using UnityEngine.Pool;

/// <summary>
/// ���� ź��(Bullet) Ŭ���� -> ź��(�ν�)
/// </summary>
public class GuidedBullet : MonoBehaviour, IBullet
{
    #region ź��(�ν�) �ʵ�
    // �� ź���� �����ϴ� ������Ʈ Ǯ
    IObjectPool<GuidedBullet> _guidedBulletPool;

    // �̵� ����
    Vector3 moveDirection;

    // �̵� �ӵ�
    public float speed = 3f;
    #endregion

    /// <summary>
    /// Ǯ �Ŵ����κ��� �ڽ��� ������ Ǯ�� �Ҵ�
    /// </summary>
    public void SetPool<T>(IObjectPool<T> pool) where T : Component
    {
        _guidedBulletPool = pool as IObjectPool<GuidedBullet>;
    }

    #region Update, OnTriggerEnter
    void Update()
    {
        // TODO: ���� PUN2 ����ؼ� ���� ����
        transform.position += moveDirection * speed * Time.deltaTime;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponentInParent<PlayerController>();

            if (player.isGroggyAndinvincibleState == false)
            {
                player.HitBullet();
                _guidedBulletPool?.Release(this);
            }
        }
        else if (other.CompareTag("Structures"))
        {
            _guidedBulletPool?.Release(this);
        }
    }
    #endregion

    #region ���� �� ����� �Լ���
    /// <summary>
    /// Ǯ���� ������ �� ȣ��Ǵ� �ʱ�ȭ �Լ�
    /// </summary>
    public void OnSpawn()
    {
        // TODO: ���� ����Ʈ �Ǵ� ���� �ʱ�ȭ ó��
    }

    /// <summary>
    /// �߻�� �� �̵� ������ ����
    /// </summary>
    public void Initialize(Vector3 direction)
    {
        moveDirection = direction.normalized;
    }
    #endregion
}
