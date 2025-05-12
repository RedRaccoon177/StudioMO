using UnityEngine;
using UnityEngine.Pool;

/// <summary>
/// �Ϲ� ź��(Bullet) Ŭ����
/// Ư�� �������� �̵��ϴٰ� Player �Ǵ� Structures�� �浹�ϸ� Ǯ�� �ݳ��ȴ�
/// </summary>
public class NormalBullet : MonoBehaviour, IBullet
{
    // �� ź���� �����ϴ� ������Ʈ Ǯ
    IObjectPool<NormalBullet> _normalBulletPool;

    // �̵� ���� (�׻� ����ȭ�� ����)
    Vector3 moveDirection;

    // �̵� �ӵ� (�ʴ� �Ÿ�)
    public float speed = 3f;

    /// <summary>
    /// Ǯ �Ŵ����κ��� �ڽ��� ������ Ǯ�� �Ҵ�޴´�
    /// </summary>
    /// <typeparam name="T">������Ʈ Ÿ��</typeparam>
    /// <param name="pool">ObjectPool ����</param>
    public void SetPool<T>(IObjectPool<T> pool) where T : Component
    {
        _normalBulletPool = pool as IObjectPool<NormalBullet>;
    }

    /// <summary>
    /// Ǯ���� ������ �� ȣ��Ǵ� �ʱ�ȭ �Լ�
    /// ���� ����Ʈ ���, ���� ���� �ʱ�ȭ � ���� �� �ִ�
    /// ����� �� �����̴�
    /// </summary>
    public void OnSpawn()
    {
        // TODO: ���� ����Ʈ �Ǵ� ���� �ʱ�ȭ ó��
    }

    /// <summary>
    /// �߻�� �� �̵� ������ �����Ѵ�
    /// </summary>
    /// <param name="direction">�̵� ���� ����</param>
    public void Initialize(Vector3 direction)
    {
        moveDirection = direction.normalized;
    }

    /// <summary>
    /// �� ������ �̵� ó��
    /// </summary>
    void Update()
    {
        transform.position += moveDirection * speed * Time.deltaTime;
    }

    /// <summary>
    /// �ٸ� ������Ʈ�� Ʈ���� �浹 �� ȣ��
    /// Player�� Structures�� �浹�ϸ� Ǯ�� �ڽ��� �ݳ��Ѵ�
    /// </summary>
    /// <param name="other">�浹�� �ݶ��̴�</param>
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Structures"))
        {
            _normalBulletPool?.Release(this);
        }
    }
}
