using System;
using UnityEngine;
using UnityEngine.Pool;

/// <summary>
/// Bullet �������� ����ϴ� ������Ʈ Ǯ�� Ŭ����
/// </summary>
public class ObjectPoolingBullet
{
    // ����Ƽ ���� Ǯ �������̽�
    private readonly IObjectPool<Bullet> _pool;

    /// <summary>
    /// ������: Bullet �������� Ǯ�� ������ ������ �ʱ�ȭ
    /// </summary>
    public ObjectPoolingBullet(Bullet prefab, Transform parent = null, int defaultCapacity = 10, int maxSize = 100)
    {
        _pool = new ObjectPool<Bullet>(
            // ������ �� ���ο� Bullet ����
            createFunc: () =>
            {
                Bullet obj = UnityEngine.Object.Instantiate(prefab, parent);

                //// IPoolable ���� ��, �ش� Ǯ ����
                //if (obj.TryGetComponent(out IPoolable<Bullet> poolable))
                //{
                //    poolable.SetPool(_pool);
                //}

                return obj;
            },

            // ������ �� ������Ʈ Ȱ��ȭ

            actionOnGet: obj => obj.gameObject.SetActive(true),

            // �ݳ��� �� ������Ʈ ��Ȱ��ȭ
            actionOnRelease: obj => obj.gameObject.SetActive(false),

            // ������ �� ������Ʈ �ı�
            actionOnDestroy: obj => UnityEngine.Object.Destroy(obj.gameObject),

            // �ߺ� �ݳ� �˻� ��Ȱ��ȭ (���� ��)
            collectionCheck: false,

            // �ʱ� �뷮�� �ִ� ũ�� ����
            defaultCapacity: defaultCapacity,
            maxSize: maxSize
        );
    }

    /// <summary>
    /// Bullet�� Ǯ���� �����ɴϴ�
    /// </summary>
    public Bullet Get() => _pool.Get();

    /// <summary>
    /// Bullet�� Ǯ�� �ݳ��մϴ�
    /// </summary>
    public void Release(Bullet obj) => _pool.Release(obj);
}
