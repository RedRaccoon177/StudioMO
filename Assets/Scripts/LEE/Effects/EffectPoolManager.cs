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
        public string effectName;                  // 이펙트 이름 키 (문자열로 접근)
        public GameObject prefab;                  // 이펙트 프리팹
        public float autoReleaseTime = 1.0f;       // 자동으로 꺼질 시간 (초)
        public int defaultCapacity = 10;           // 풀 초기에 생성해둘 개수
        public int maxSize = 100;                  // 최대 보유 가능 개수
    }

    [Header("풀링할 이펙트 프리팹 목록")]
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
    /// 이펙트 풀 생성 (설정 통째로 받아서 적용)
    /// </summary>
    public void CreatePool(EffectPrefab entry)
    {
        if (_pools.ContainsKey(entry.effectName))
        {
            Debug.LogWarning($"[EffectPool] 중복 이름 발견: '{entry.effectName}'");
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
    /// 이펙트를 풀에서 꺼내 위치에 배치하고 일정 시간 후 자동 반납
    /// </summary>
    public void SpawnEffect(string name, Vector3 pos, Quaternion rot)
    {
        if (!_pools.TryGetValue(name, out var pool))
        {
            Debug.LogWarning($"[EffectPool] '{name}' 이펙트 풀이 없음");
            return;
        }

        var effect = pool.Get();
        effect.transform.SetPositionAndRotation(pos, rot);

        StartCoroutine(AutoRelease(effect, name));
    }

    /// <summary>
    /// 일정 시간 후 자동으로 해당 이펙트를 풀에 반납
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
