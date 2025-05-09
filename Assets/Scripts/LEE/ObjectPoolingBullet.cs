using System;
using UnityEngine;
using UnityEngine.Pool;

/// <summary>
/// Bullet ������Ʈ�� Ưȭ�� Ǯ�� �Ŵ��� Ŭ����
/// MonoBehaviour�� �Ҵ��Ͽ� ����Ƽ ������ ���
/// </summary>
public class ObjectPoolingBullet : MonoBehaviour
{
    #region ź�� ���� �ʵ�
    [Header("Bullet ���� �ʵ�")]
    [Tooltip("Ǯ���� ����� Bullet ������")]
    public Bullet bulletPrefab;

    [Tooltip("Ǯ���� ������Ʈ�� �θ� Transform (����)")]
    public Transform poolParent;

    [Tooltip("�⺻ Ǯ �뷮")]
    int defaultCapacity = 50;

    [Tooltip("�ִ� Ǯ �뷮")]
    int maxSize = 200;
    #endregion

    // ����Ƽ ���� ObjectPool ���
    IObjectPool<Bullet> _pool;

    /// <summary>
    /// ���� ���� �� Ǯ �ʱ�ȭ
    /// </summary>
    void Awake()
    {
        // ObjectPool ����
        _pool = new ObjectPool<Bullet>(
            createFunc: CreateBullet,
            actionOnGet: OnGetFromPool,
            actionOnRelease: OnReleaseToPool,
            actionOnDestroy: OnDestroyFromPool,
            collectionCheck: false,
            defaultCapacity: defaultCapacity,
            maxSize: maxSize
        );
    }

    /// <summary>
    /// Bullet �ν��Ͻ��� ���� ������ �� ȣ��
    /// </summary>
    Bullet CreateBullet()
    {
        var bullet = Instantiate(bulletPrefab, poolParent);
        bullet.SetPool(_pool); // Bullet�� IPoolable �������̽��� �����Ѵٰ� ����
        return bullet;
    }

    /// <summary>
    /// Ǯ���� ������Ʈ�� ���� �� ȣ��
    /// </summary>
    void OnGetFromPool(Bullet bullet)
    {
        bullet.gameObject.SetActive(true);
        bullet.OnSpawn(); // Bullet�� �ʿ��ϸ� �� �޼��� ���� ����
    }

    /// <summary>
    /// Ǯ�� ������Ʈ�� �ݳ��� �� ȣ��
    /// </summary>
    void OnReleaseToPool(Bullet bullet)
    {
        bullet.gameObject.SetActive(false);
    }

    /// <summary>
    /// Ǯ���� ������ ������ �� ȣ��
    /// </summary>
    void OnDestroyFromPool(Bullet bullet)
    {
        Destroy(bullet.gameObject);
    }


    //�̰� �Ʒ��� �ؼ��ؾ���.

    /// <summary>
    /// �ܺο��� Bullet �ν��Ͻ��� ��û
    /// </summary>
    public Bullet GetBullet() => _pool.Get();

    /// <summary>
    /// �ܺο��� Bullet �ν��Ͻ��� Ǯ�� �ݳ�
    /// </summary>
    public void ReleaseBullet(Bullet bullet) => _pool.Release(bullet);
}
