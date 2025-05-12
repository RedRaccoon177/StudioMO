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
/// UnityEngine.Pool �ý����� Ȱ���Ͽ� ź���� ������ �Ҹ��� ����ȭ�Ѵ�
/// </summary>
public class ObjectPoolingBullet : MonoBehaviour
{
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

    /// <summary>
    /// Ư�� Ÿ���� ź�� Ǯ�� �����Ͽ� ����Ѵ�
    /// Ǯ�� �������� ������ ���� Instantiate�Ͽ� �����Ѵ�
    /// 
    /// 
    /// </summary>
    /// <typeparam name="T">ź���� ������Ʈ Ÿ��</typeparam>
    /// <param name="prefab">Ǯ���� ź�� ������</param>
    /// <param name="parent">������ ź�� ������Ʈ�� �θ� Ʈ������(����)</param>
    public void CreatePool<T>(T prefab, Transform parent = null) where T : Component, IBullet
    {
        ObjectPool<T> pool = null;

        pool = new ObjectPool<T>(
            createFunc: () =>
            {
                // ź�� �ν��Ͻ��� ���� ������ �Ѵ�
                // pool ��ü�� createFunc �ȿ��� ���� �Ұ��̹Ƿ� SetPool�� ���⼭ ȣ������ �ʴ´�
                T bullet = Instantiate(prefab, parent);
                return bullet;
            },
            actionOnGet: (bullet) =>
            {
                // Ǯ���� ������Ʈ�� ���� �� ȣ��
                // ���� �� Ǯ ������ �����ϰ� �ʱ�ȭ �۾��� �����Ѵ�
                bullet.SetPool(pool);
                bullet.gameObject.SetActive(true);
                bullet.OnSpawn();
            },
            actionOnRelease: (bullet) =>
            {
                // Ǯ�� ������Ʈ�� �ݳ��� �� ȣ��
                // ������Ʈ�� ��Ȱ��ȭ�Ͽ� ȭ�鿡�� �����
                bullet.gameObject.SetActive(false);
            },
            actionOnDestroy: (bullet) =>
            {
                // Ǯ�� �ִ� ũ�⸦ �ʰ��ϰų�, Ǯ ��ü�� ������ �� ȣ��
                // ������Ʈ�� ������ �ı��Ͽ� �޸𸮿��� �����Ѵ�
                Destroy(bullet.gameObject);
            },
            collectionCheck: false, // �ߺ� �ݳ� �˻� ��Ȱ��ȭ (���� �켱)
            defaultCapacity: defaultCapacity, // �ʱ� ���� ����
            maxSize: maxSize // �ִ� Ǯ ���� ����
        );

        // Ÿ�Ժ� Ǯ ��ųʸ��� ���
        _pools[typeof(T)] = pool;
    }

    /// <summary>
    /// Ư�� Ÿ���� ź�� ������Ʈ�� Ǯ���� ������
    /// Ǯ�� ������ ���� �α׸� ����Ѵ�
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
            Debug.LogError($"[ObjectPoolingBullet] Ǯ�� ã�� �� �����ϴ�: {typeof(T)}");
            return null;
        }
    }

    /// <summary>
    /// Ư�� Ÿ���� ź�� ������Ʈ�� Ǯ�� �ݳ��Ѵ�
    /// Ǯ�� ������ ���� �α׸� ����Ѵ�
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
            Debug.LogError($"[ObjectPoolingBullet] Ǯ�� ã�� �� �����ϴ�: {typeof(T)}");
        }
    }
}
