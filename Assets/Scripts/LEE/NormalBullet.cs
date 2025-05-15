using UnityEngine;
using UnityEngine.Pool;

/// <summary>
/// 일반 탄막(Bullet) 클래스 -> 탄막(비인식)
/// </summary>
public class NormalBullet : MonoBehaviour, IBullet
{
    #region 탄막(비인식) 필드
    // 이 탄막을 관리하는 오브젝트 풀
    IObjectPool<NormalBullet> _normalBulletPool;

    // 이동 방향
    Vector3 moveDirection;

    // 이동 속도
    public float speed = 3f;
    #endregion

    /// <summary>
    /// 풀 매니저로부터 자신을 관리할 풀을 할당
    /// </summary>
    /// <typeparam name="T">컴포넌트 타입</typeparam>
    /// <param name="pool">ObjectPool 참조</param>
    public void SetPool<T>(IObjectPool<T> pool) where T : Component
    {
        _normalBulletPool = pool as IObjectPool<NormalBullet>;
    }

    #region Update, OnTriggerEnter
    void Update()
    {
        //TODO: PUN2 고려해서 추후 수정될 예정
        transform.position += moveDirection * speed * Time.deltaTime;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponentInParent<PlayerController>();

            if (player.isGroggyAndinvincibleState == false)
            {
                player.HitBullet();
                _normalBulletPool?.Release(this);
            }
        }
        else if (other.CompareTag("Structures"))
        {
            _normalBulletPool?.Release(this);
        }
    }
    #endregion

    #region 생성 시 실행될 함수들
    /// <summary>
    /// 풀에서 꺼내질 때 호출되는 초기화 함수
    /// </summary>
    public void OnSpawn()
    {
        // TODO: 생성 이펙트 또는 상태 초기화 처리
    }

    /// <summary>
    /// 발사될 때 이동 방향을 설정
    /// </summary>
    /// <param name="direction">이동 방향 벡터</param>
    public void Initialize(Vector3 direction)
    {
        moveDirection = direction.normalized;
    }
    #endregion
}
