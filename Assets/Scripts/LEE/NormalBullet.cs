using UnityEngine;
using UnityEngine.Pool;

/// <summary>
/// �Ϲ� ź��(Bullet) Ŭ���� -> ź��(���ν�)
/// </summary>
public class NormalBullet : MonoBehaviour, IBullet
{
    #region ź��(���ν�) �ʵ�
    // �� ź���� �����ϴ� ������Ʈ Ǯ
    IObjectPool<NormalBullet> _normalBulletPool;

    // �̵� ����
    Vector3 moveDirection;

    // �̵� �ӵ�
    public float speed = 3f;
    #endregion

    /// <summary>
    /// Ǯ �Ŵ����κ��� �ڽ��� ������ Ǯ�� �Ҵ�
    /// </summary>
    /// <typeparam name="T">������Ʈ Ÿ��</typeparam>
    /// <param name="pool">ObjectPool ����</param>
    public void SetPool<T>(IObjectPool<T> pool) where T : Component
    {
        _normalBulletPool = pool as IObjectPool<NormalBullet>;
    }

    #region Update, OnTriggerEnter
    void Update()
    {
        //TODO: PUN2 ����ؼ� ���� ������ ����
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
                _normalBulletPool?.Release(this);
            }
        }
        else if (other.CompareTag("Structures"))
        {
            _normalBulletPool?.Release(this);
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
    /// <param name="direction">�̵� ���� ����</param>
    public void Initialize(Vector3 direction)
    {
        moveDirection = direction.normalized;
    }
    #endregion
}
