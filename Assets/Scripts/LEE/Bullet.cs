using UnityEngine;
using UnityEngine.Pool;

/// <summary>
/// Bullet 클래스: 오브젝트 풀링 기반 탄막 로직
/// 이동, 충돌 처리, 풀 반납까지 담당
/// </summary>
public class Bullet : MonoBehaviour
{
    // 이 Bullet 인스턴스를 관리하는 풀 참조
    IObjectPool<Bullet> _pool;

    // 이동 방향 (정규화된 벡터)
    Vector3 moveDirection;

    // 총알 이동 속도 (초당 거리)
    public float speed = 3f;

    // TODO: 삭제 시 1초간 대기 등 딜레이 반납 처리 구현 예정

    /// <summary>
    /// 이 Bullet을 어떤 풀에서 생성했는지 설정하는 함수
    /// 풀링 매니저에서 생성 직후 호출됨
    /// </summary>
    public void SetPool(IObjectPool<Bullet> pool)
    {
        _pool = pool;
    }

    /// <summary>
    /// 외부에서 발사 시 호출되는 초기화 함수
    /// 이동 방향을 설정함
    /// </summary>
    public void Initialize(Vector3 direction)
    {
        moveDirection = direction.normalized;
    }

    /// <summary>
    /// 풀에서 꺼내졌을 때 자동 호출되는 초기화 함수 (현재는 미사용)
    /// 이펙트 재생, 상태 초기화 등 후처리를 여기에 넣을 수 있음
    /// </summary>
    public void OnSpawn()
    {
        // TODO: 생성 이펙트 또는 상태 초기화 처리 가능

        // TODO: 이동 시작을 1초 지연하려면 코루틴 또는 시간 체크 필요
    }

    void Update()
    {
        // 이동 방향으로 매 프레임 이동
        transform.position += moveDirection * speed * Time.deltaTime;
    }

    void OnCollisionEnter(Collision collision)
    {
        string tag = collision.gameObject.tag;

        // 충돌한 오브젝트가 Player 또는 Structures일 경우
        if (tag == "Player" || tag == "Structures")
        {
            // TODO: 데미지 처리, 이펙트 등 추가 가능

            // 풀에 자기 자신을 반납 (비활성화 후 재사용 대기)
            _pool?.Release(this);
        }
    }
}
