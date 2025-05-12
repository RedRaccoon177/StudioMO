using UnityEngine;
using UnityEngine.Pool;

/// <summary>
/// 일반 탄막(Bullet) 클래스
/// 특정 방향으로 이동하다가 Player 또는 Structures와 충돌하면 풀에 반납된다
/// </summary>
public class NormalBullet : MonoBehaviour, IBullet
{
    // 이 탄막을 관리하는 오브젝트 풀
    IObjectPool<NormalBullet> _normalBulletPool;

    // 이동 방향 (항상 정규화된 벡터)
    Vector3 moveDirection;

    // 이동 속도 (초당 거리)
    public float speed = 3f;

    /// <summary>
    /// 풀 매니저로부터 자신을 관리할 풀을 할당받는다
    /// </summary>
    /// <typeparam name="T">컴포넌트 타입</typeparam>
    /// <param name="pool">ObjectPool 참조</param>
    public void SetPool<T>(IObjectPool<T> pool) where T : Component
    {
        _normalBulletPool = pool as IObjectPool<NormalBullet>;
    }

    /// <summary>
    /// 풀에서 꺼내질 때 호출되는 초기화 함수
    /// 생성 이펙트 재생, 내부 상태 초기화 등에 사용될 수 있다
    /// 현재는 빈 구현이다
    /// </summary>
    public void OnSpawn()
    {
        // TODO: 생성 이펙트 또는 상태 초기화 처리
    }

    /// <summary>
    /// 발사될 때 이동 방향을 설정한다
    /// </summary>
    /// <param name="direction">이동 방향 벡터</param>
    public void Initialize(Vector3 direction)
    {
        moveDirection = direction.normalized;
    }

    /// <summary>
    /// 매 프레임 이동 처리
    /// </summary>
    void Update()
    {
        transform.position += moveDirection * speed * Time.deltaTime;
    }

    /// <summary>
    /// 다른 오브젝트와 트리거 충돌 시 호출
    /// Player나 Structures와 충돌하면 풀에 자신을 반납한다
    /// </summary>
    /// <param name="other">충돌한 콜라이더</param>
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Structures"))
        {
            _normalBulletPool?.Release(this);
        }
    }
}
