using System;
using UnityEngine;
using UnityEngine.Pool;

/// <summary>
/// Bullet 오브젝트에 특화된 풀링 매니저 클래스
/// MonoBehaviour에 할당하여 유니티 씬에서 사용
/// </summary>
public class ObjectPoolingBullet : MonoBehaviour
{
    #region 탄막 관련 필드
    [Header("Bullet 관련 필드")]
    [Tooltip("풀링에 사용할 Bullet 프리팹")]
    public Bullet bulletPrefab;

    [Tooltip("풀링된 오브젝트의 부모 Transform (선택)")]
    public Transform poolParent;

    [Tooltip("기본 풀 용량")]
    int defaultCapacity = 50;

    [Tooltip("최대 풀 용량")]
    int maxSize = 200;
    #endregion

    // 유니티 내장 ObjectPool 사용
    IObjectPool<Bullet> _pool;

    /// <summary>
    /// 게임 시작 시 풀 초기화
    /// </summary>
    void Awake()
    {
        // ObjectPool 생성
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
    /// Bullet 인스턴스를 새로 생성할 때 호출
    /// </summary>
    Bullet CreateBullet()
    {
        var bullet = Instantiate(bulletPrefab, poolParent);
        bullet.SetPool(_pool); // Bullet이 IPoolable 인터페이스를 구현한다고 가정
        return bullet;
    }

    /// <summary>
    /// 풀에서 오브젝트를 꺼낼 때 호출
    /// </summary>
    void OnGetFromPool(Bullet bullet)
    {
        bullet.gameObject.SetActive(true);
        bullet.OnSpawn(); // Bullet이 필요하면 이 메서드 정의 가능
    }

    /// <summary>
    /// 풀에 오브젝트를 반납할 때 호출
    /// </summary>
    void OnReleaseToPool(Bullet bullet)
    {
        bullet.gameObject.SetActive(false);
    }

    /// <summary>
    /// 풀에서 완전히 제거할 때 호출
    /// </summary>
    void OnDestroyFromPool(Bullet bullet)
    {
        Destroy(bullet.gameObject);
    }


    //이거 아래들 해석해야함.

    /// <summary>
    /// 외부에서 Bullet 인스턴스를 요청
    /// </summary>
    public Bullet GetBullet() => _pool.Get();

    /// <summary>
    /// 외부에서 Bullet 인스턴스를 풀에 반납
    /// </summary>
    public void ReleaseBullet(Bullet bullet) => _pool.Release(bullet);
}
