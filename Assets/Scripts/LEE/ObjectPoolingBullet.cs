using System;
using UnityEngine;
using UnityEngine.Pool;

/// <summary>
/// Bullet 전용으로 사용하는 오브젝트 풀링 클래스
/// </summary>
public class ObjectPoolingBullet
{
    // 유니티 내장 풀 인터페이스
    private readonly IObjectPool<Bullet> _pool;

    /// <summary>
    /// 생성자: Bullet 프리팹을 풀링 가능한 구조로 초기화
    /// </summary>
    public ObjectPoolingBullet(Bullet prefab, Transform parent = null, int defaultCapacity = 10, int maxSize = 100)
    {
        _pool = new ObjectPool<Bullet>(
            // 부족할 때 새로운 Bullet 생성
            createFunc: () =>
            {
                Bullet obj = UnityEngine.Object.Instantiate(prefab, parent);

                //// IPoolable 구현 시, 해당 풀 주입
                //if (obj.TryGetComponent(out IPoolable<Bullet> poolable))
                //{
                //    poolable.SetPool(_pool);
                //}

                return obj;
            },

            // 가져올 때 오브젝트 활성화

            actionOnGet: obj => obj.gameObject.SetActive(true),

            // 반납할 때 오브젝트 비활성화
            actionOnRelease: obj => obj.gameObject.SetActive(false),

            // 제거할 때 오브젝트 파괴
            actionOnDestroy: obj => UnityEngine.Object.Destroy(obj.gameObject),

            // 중복 반납 검사 비활성화 (성능 ↑)
            collectionCheck: false,

            // 초기 용량과 최대 크기 설정
            defaultCapacity: defaultCapacity,
            maxSize: maxSize
        );
    }

    /// <summary>
    /// Bullet을 풀에서 가져옵니다
    /// </summary>
    public Bullet Get() => _pool.Get();

    /// <summary>
    /// Bullet을 풀에 반납합니다
    /// </summary>
    public void Release(Bullet obj) => _pool.Release(obj);
}
