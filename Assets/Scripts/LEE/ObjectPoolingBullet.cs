using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

/// <summary>
/// Bullet 종류가 공통으로 가져야 할 기능을 정의하는 인터페이스
/// </summary>
public interface IBullet
{
    /// <summary>
    /// 풀 매니저로부터 자신을 관리할 풀을 할당받는 함수
    /// </summary>
    /// <typeparam name="T">컴포넌트 타입</typeparam>
    /// <param name="pool">자신을 관리할 ObjectPool 참조</param>
    void SetPool<T>(IObjectPool<T> pool) where T : Component;

    /// <summary>
    /// 풀에서 꺼내졌을 때 호출되는 초기화 함수
    /// </summary>
    void OnSpawn();
}

/// <summary>
/// 다양한 종류의 탄막을 통합 관리하는 오브젝트 풀 매니저 클래스
/// </summary>
public class ObjectPoolingBullet : MonoBehaviour
{
    #region 오브젝트 풀링 탄막 필드
    [Header("풀링 설정값")]
    [Tooltip("초기 풀에 미리 생성할 오브젝트 수")]
    [SerializeField] int defaultCapacity = 50;

    [Tooltip("풀에 저장할 수 있는 최대 오브젝트 수")]
    [SerializeField] int maxSize = 200;

    /// <summary>
    /// 탄막 타입별 풀을 저장하는 딕셔너리
    /// 타입을 키로 하여 각 탄막 풀을 관리한다
    /// </summary>
    private Dictionary<Type, object> _pools = new Dictionary<Type, object>();
    #endregion

    /// <summary>
    /// 특정 타입의 탄막 풀을 생성하여 등록하고 풀에 존재하지 않으면 새로 Instantiate
    /// </summary>
    /// <typeparam name="T">탄막의 컴포넌트 타입</typeparam>
    /// <param name="prefab">풀링할 탄막 프리팹</param>
    /// <param name="parent">생성된 탄막 오브젝트의 부모 트랜스폼(선택)</param>
    public void CreatePool<T>(T prefab, Transform parent = null) where T : Component, IBullet
    {
        ObjectPool<T> pool = null;

        pool = new ObjectPool<T>
            (
                createFunc: () =>
                {
                    // 탄막 인스턴스를 새로 생성
                    T bullet = Instantiate(prefab, parent);
                    return bullet;
                },
                actionOnGet: (bullet) =>
                {
                    // 풀에서 오브젝트를 꺼낼 때 호출 + 꺼낼 때 풀 정보를 주입하여 초기화 작업 진행
                    bullet.SetPool(pool);
                    bullet.gameObject.SetActive(true);
                    bullet.OnSpawn();
                },
                actionOnRelease: (bullet) =>
                {
                    // 풀에 오브젝트를 반납할 때 호출 + 오브젝트 비활성화
                    bullet.gameObject.SetActive(false);
                },
                actionOnDestroy: (bullet) =>
                {
                    Destroy(bullet.gameObject);
                },
                collectionCheck: false, // 중복 반납 검사 비활성화 (성능 우선)
                defaultCapacity: defaultCapacity, // 초기 생성 개수
                maxSize: maxSize // 최대 풀 보유 개수
            );

        // 타입별 풀 딕셔너리에 등록
        _pools[typeof(T)] = pool;
    }

    #region 오브젝트 풀링 풀 in, out
    /// <summary>
    /// 특정 타입의 탄막 오브젝트를 풀에서 꺼낸다 + 풀에 없으면 에러 로그
    /// </summary>
    /// <typeparam name="T">탄막의 컴포넌트 타입</typeparam>
    /// <returns>풀에서 꺼낸 탄막 인스턴스</returns>
    public T GetBullet<T>() where T : Component, IBullet
    {
        if (_pools.TryGetValue(typeof(T), out var pool))
        {
            return (pool as ObjectPool<T>).Get();
        }
        else
        {
            Debug.LogError($"[ObjectPoolingBullet] 풀을 찾을 수 없다: {typeof(T)}");
            return null;
        }
    }

    /// <summary>
    /// 특정 타입의 탄막 오브젝트를 풀로 반납 + 풀에 없으면 에러 로그
    /// </summary>
    /// <typeparam name="T">탄막의 컴포넌트 타입</typeparam>
    /// <param name="bullet">반납할 탄막 인스턴스</param>
    public void ReleaseBullet<T>(T bullet) where T : Component, IBullet
    {
        if (_pools.TryGetValue(typeof(T), out var pool))
        {
            (pool as ObjectPool<T>).Release(bullet);
        }
        else
        {
            Debug.LogError($"[ObjectPoolingBullet] 풀을 찾을 수 없다.: {typeof(T)}");
        }
    }
    #endregion
}
