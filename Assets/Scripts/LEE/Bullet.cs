using UnityEngine;
using UnityEngine.Pool;

public class Bullet : MonoBehaviour
{
    IObjectPool<Bullet> _pool;
    Vector3 moveDirection;
    public float speed = 3f;

    // 생성 후 1초간 대기

    // 삭제 시 1초간 대기

    public void SetPool(IObjectPool<Bullet> pool)
    {
        _pool = pool;
    }

    /// <summary>
    /// 이동 방향을 외부에서 지정
    /// </summary>
    public void Initialize(Vector3 direction)
    {
        moveDirection = direction.normalized;
    }

    /// <summary>
    /// 풀에서 꺼내질 때 호출 (초기화 등)
    /// </summary>
    public void OnSpawn()
    {
        // 필요 시 초기화 (예: 이펙트 재생)
    }

    void Update()
    {
        transform.position += moveDirection * speed * Time.deltaTime;
    }

    void OnCollisionEnter(Collision collision)
    {
        string tag = collision.gameObject.tag;

        if (tag == "Player" || tag == "Structures")
        {
            // TODO: 필요시 추가 로직
            _pool?.Release(this);
        }
    }
}
