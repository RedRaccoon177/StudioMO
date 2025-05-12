using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

/// <summary>
/// Bullet ������ �������� ������ �� ����� �����ϴ� �������̽�
/// </summary>
public interface IBullet
{
    /// <summary>
    /// Ǯ �Ŵ����κ��� �ڽ��� ������ Ǯ�� �Ҵ�޴� �Լ�
    /// </summary>
    /// <typeparam name="T">������Ʈ Ÿ��</typeparam>
    /// <param name="pool">�ڽ��� ������ ObjectPool ����</param>
    void SetPool<T>(IObjectPool<T> pool) where T : Component;

    /// <summary>
    /// Ǯ���� �������� �� ȣ��Ǵ� �ʱ�ȭ �Լ�
    /// </summary>
    void OnSpawn();
}

/// <summary>
/// �پ��� ������ ź���� ���� �����ϴ� ������Ʈ Ǯ �Ŵ��� Ŭ����
/// </summary>
public class ObjectPoolingBullet : MonoBehaviour
{
    #region ������Ʈ Ǯ�� ź�� �ʵ�
    [Header("Ǯ�� ������")]
    [Tooltip("�ʱ� Ǯ�� �̸� ������ ������Ʈ ��")]
    [SerializeField] int defaultCapacity = 50;

    [Tooltip("Ǯ�� ������ �� �ִ� �ִ� ������Ʈ ��")]
    [SerializeField] int maxSize = 200;

    /// <summary>
    /// ź�� Ÿ�Ժ� Ǯ�� �����ϴ� ��ųʸ�
    /// Ÿ���� Ű�� �Ͽ� �� ź�� Ǯ�� �����Ѵ�
    /// </summary>
    private Dictionary<Type, object> _pools = new Dictionary<Type, object>();
    #endregion

    /// <summary>
    /// Ư�� Ÿ���� ź�� Ǯ�� �����Ͽ� ����ϰ� Ǯ�� �������� ������ ���� Instantiate
    /// </summary>
    /// <typeparam name="T">ź���� ������Ʈ Ÿ��</typeparam>
    /// <param name="prefab">Ǯ���� ź�� ������</param>
    /// <param name="parent">������ ź�� ������Ʈ�� �θ� Ʈ������(����)</param>
    public void CreatePool<T>(T prefab, Transform parent = null) where T : Component, IBullet
    {
        ObjectPool<T> pool = null;

        pool = new ObjectPool<T>
            (
                createFunc: () =>
                {
                    // ź�� �ν��Ͻ��� ���� ����
                    T bullet = Instantiate(prefab, parent);
                    return bullet;
                },
                actionOnGet: (bullet) =>
                {
                    // Ǯ���� ������Ʈ�� ���� �� ȣ�� + ���� �� Ǯ ������ �����Ͽ� �ʱ�ȭ �۾� ����
                    bullet.SetPool(pool);
                    bullet.gameObject.SetActive(true);
                    bullet.OnSpawn();
                },
                actionOnRelease: (bullet) =>
                {
                    // Ǯ�� ������Ʈ�� �ݳ��� �� ȣ�� + ������Ʈ ��Ȱ��ȭ
                    bullet.gameObject.SetActive(false);
                },
                actionOnDestroy: (bullet) =>
                {
                    Destroy(bullet.gameObject);
                },
                collectionCheck: false, // �ߺ� �ݳ� �˻� ��Ȱ��ȭ (���� �켱)
                defaultCapacity: defaultCapacity, // �ʱ� ���� ����
                maxSize: maxSize // �ִ� Ǯ ���� ����
            );

        // Ÿ�Ժ� Ǯ ��ųʸ��� ���
        _pools[typeof(T)] = pool;
    }

    #region ������Ʈ Ǯ�� Ǯ in, out
    /// <summary>
    /// Ư�� Ÿ���� ź�� ������Ʈ�� Ǯ���� ������ + Ǯ�� ������ ���� �α�
    /// </summary>
    /// <typeparam name="T">ź���� ������Ʈ Ÿ��</typeparam>
    /// <returns>Ǯ���� ���� ź�� �ν��Ͻ�</returns>
    public T GetBullet<T>() where T : Component, IBullet
    {
        if (_pools.TryGetValue(typeof(T), out var pool))
        {
            return (pool as ObjectPool<T>).Get();
        }
        else
        {
            Debug.LogError($"[ObjectPoolingBullet] Ǯ�� ã�� �� ����: {typeof(T)}");
            return null;
        }
    }

    /// <summary>
    /// Ư�� Ÿ���� ź�� ������Ʈ�� Ǯ�� �ݳ� + Ǯ�� ������ ���� �α�
    /// </summary>
    /// <typeparam name="T">ź���� ������Ʈ Ÿ��</typeparam>
    /// <param name="bullet">�ݳ��� ź�� �ν��Ͻ�</param>
    public void ReleaseBullet<T>(T bullet) where T : Component, IBullet
    {
        if (_pools.TryGetValue(typeof(T), out var pool))
        {
            (pool as ObjectPool<T>).Release(bullet);
        }
        else
        {
            Debug.LogError($"[ObjectPoolingBullet] Ǯ�� ã�� �� ����.: {typeof(T)}");
        }
    }
    #endregion
}
