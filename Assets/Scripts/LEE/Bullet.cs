using UnityEngine;
using UnityEngine.Pool;

/// <summary>
/// Bullet Ŭ����: ������Ʈ Ǯ�� ��� ź�� ����
/// �̵�, �浹 ó��, Ǯ �ݳ����� ���
/// </summary>
public class Bullet : MonoBehaviour
{
    // �� Bullet �ν��Ͻ��� �����ϴ� Ǯ ����
    IObjectPool<Bullet> _pool;

    // �̵� ���� (����ȭ�� ����)
    Vector3 moveDirection;

    // �Ѿ� �̵� �ӵ� (�ʴ� �Ÿ�)
    public float speed = 3f;

    // TODO: ���� �� 1�ʰ� ��� �� ������ �ݳ� ó�� ���� ����

    /// <summary>
    /// �� Bullet�� � Ǯ���� �����ߴ��� �����ϴ� �Լ�
    /// Ǯ�� �Ŵ������� ���� ���� ȣ���
    /// </summary>
    public void SetPool(IObjectPool<Bullet> pool)
    {
        _pool = pool;
    }

    /// <summary>
    /// �ܺο��� �߻� �� ȣ��Ǵ� �ʱ�ȭ �Լ�
    /// �̵� ������ ������
    /// </summary>
    public void Initialize(Vector3 direction)
    {
        moveDirection = direction.normalized;
    }

    /// <summary>
    /// Ǯ���� �������� �� �ڵ� ȣ��Ǵ� �ʱ�ȭ �Լ� (����� �̻��)
    /// ����Ʈ ���, ���� �ʱ�ȭ �� ��ó���� ���⿡ ���� �� ����
    /// </summary>
    public void OnSpawn()
    {
        // TODO: ���� ����Ʈ �Ǵ� ���� �ʱ�ȭ ó�� ����

        // TODO: �̵� ������ 1�� �����Ϸ��� �ڷ�ƾ �Ǵ� �ð� üũ �ʿ�
    }

    void Update()
    {
        // �̵� �������� �� ������ �̵�
        transform.position += moveDirection * speed * Time.deltaTime;
    }

    void OnCollisionEnter(Collision collision)
    {
        string tag = collision.gameObject.tag;

        // �浹�� ������Ʈ�� Player �Ǵ� Structures�� ���
        if (tag == "Player" || tag == "Structures")
        {
            // TODO: ������ ó��, ����Ʈ �� �߰� ����

            // Ǯ�� �ڱ� �ڽ��� �ݳ� (��Ȱ��ȭ �� ���� ���)
            _pool?.Release(this);
        }
    }
}
