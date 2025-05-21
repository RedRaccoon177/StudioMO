using UnityEngine;
using UnityEngine.Pool;

/// <summary>
/// 유도 탄막(Bullet) 클래스 -> 탄막(인식)
/// </summary>
public class GuidedBullet : MonoBehaviour, IBullet
{
    #region 탄막(인식) 필드
    // 이 탄막을 관리하는 오브젝트 풀
    IObjectPool<GuidedBullet> _guidedBulletPool;

    // 이동 방향
    Vector3 moveDirection;

    // 이동 속도
    public float speed = 3f;
    #endregion

    /// <summary>
    /// 풀 매니저로부터 자신을 관리할 풀을 할당
    /// </summary>
    public void SetPool<T>(IObjectPool<T> pool) where T : Component
    {
        _guidedBulletPool = pool as IObjectPool<GuidedBullet>;
    }

    #region Update, OnTriggerEnter
    void Update()
    {
        // TODO: 추후 PUN2 고려해서 변경 가능
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
                _guidedBulletPool?.Release(this);
            }
        }
        else if (other.CompareTag("Structures"))
        {
            _guidedBulletPool?.Release(this);
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
    public void Initialize(Vector3 direction)
    {
        moveDirection = direction.normalized;
    }
    #endregion
}
