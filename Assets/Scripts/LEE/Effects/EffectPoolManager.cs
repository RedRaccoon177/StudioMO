using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;

public class EffectPoolManager : MonoBehaviour
{
    [System.Serializable]
    public class EffectPrefab
    {
        public string effectName;                  // ����Ʈ �̸� Ű (���ڿ��� ����)
        public GameObject prefab;                  // ����Ʈ ������
        public float autoReleaseTime = 1.0f;       // �ڵ����� ���� �ð� (��)
        public int defaultCapacity = 10;           // Ǯ �ʱ⿡ �����ص� ����
        public int maxSize = 100;                  // �ִ� ���� ���� ����
    }

    [Header("Ǯ���� ����Ʈ ������ ���")]
    public EffectPrefab[] effects;

    private Dictionary<string, ObjectPool<GameObject>> _pools = new();

    void Awake()
    {
        foreach (var entry in effects)
        {
            CreatePool(entry);
        }
    }

    /// <summary>
    /// ����Ʈ Ǯ ���� (���� ��°�� �޾Ƽ� ����)
    /// </summary>
    public void CreatePool(EffectPrefab entry)
    {
        if (_pools.ContainsKey(entry.effectName))
        {
            Debug.LogWarning($"[EffectPool] �ߺ� �̸� �߰�: '{entry.effectName}'");
            return;
        }

        var pool = new ObjectPool<GameObject>(
            createFunc: () =>
            {
                var obj = Instantiate(entry.prefab);
                obj.SetActive(false);
                return obj;
            },
            actionOnGet: obj =>
            {
                obj.SetActive(true);
            },
            actionOnRelease: obj =>
            {
                obj.SetActive(false);
            },
            actionOnDestroy: obj =>
            {
                Destroy(obj);
            },
            collectionCheck: false,
            defaultCapacity: entry.defaultCapacity,
            maxSize: entry.maxSize
        );

        _pools[entry.effectName] = pool;
    }

    /// <summary>
    /// ����Ʈ�� Ǯ���� ���� ��ġ�� ��ġ�ϰ� ���� �ð� �� �ڵ� �ݳ�
    /// </summary>
    public void SpawnEffect(string name, Vector3 pos, Quaternion rot)
    {
        if (!_pools.TryGetValue(name, out var pool))
        {
            Debug.LogWarning($"[EffectPool] '{name}' ����Ʈ Ǯ�� ����");
            return;
        }

        var effect = pool.Get();
        effect.transform.SetPositionAndRotation(pos, rot);

        StartCoroutine(AutoRelease(effect, name));
    }

    /// <summary>
    /// ���� �ð� �� �ڵ����� �ش� ����Ʈ�� Ǯ�� �ݳ�
    /// </summary>
    private IEnumerator AutoRelease(GameObject obj, string name)
    {
        float delay = effects.FirstOrDefault(e => e.effectName == name)?.autoReleaseTime ?? 1f;
        yield return new WaitForSeconds(delay);

        if (_pools.TryGetValue(name, out var pool))
        {
            pool.Release(obj);
        }
    }
}
