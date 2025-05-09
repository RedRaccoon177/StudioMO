using UnityEngine;
using UnityEngine.Pool;

public class Bullet : MonoBehaviour
{
    IObjectPool<Bullet> _pool;
    Vector3 moveDirection;
    public float speed = 3f;

    // ���� �� 1�ʰ� ���

    // ���� �� 1�ʰ� ���

    public void SetPool(IObjectPool<Bullet> pool)
    {
        _pool = pool;
    }

    /// <summary>
    /// �̵� ������ �ܺο��� ����
    /// </summary>
    public void Initialize(Vector3 direction)
    {
        moveDirection = direction.normalized;
    }

    /// <summary>
    /// Ǯ���� ������ �� ȣ�� (�ʱ�ȭ ��)
    /// </summary>
    public void OnSpawn()
    {
        // �ʿ� �� �ʱ�ȭ (��: ����Ʈ ���)
    }

    void Update()
    {
        transform.position += moveDirection * speed * Time.deltaTime;
    }

    void OnCollisionEnter(Collision collision)
    {
        string tag = collision.gameObject.tag;

        if (tag == "Player" || tag == "Structures")
        {
            // TODO: �ʿ�� �߰� ����
            _pool?.Release(this);
        }
    }
}
