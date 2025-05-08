using UnityEngine;
using UnityEngine.Pool;

public class Bullet : MonoBehaviour
{
    //유니티 내부 오브젝트 풀링 사용
    IObjectPool<Bullet> _pool;

    public void SetPool(IObjectPool<Bullet> pool)
    {
        _pool = pool;
    }

    void OnCollisionEnter(Collision collision)
    {
        string tag = collision.gameObject.tag;

        //플레이어 or 구조물 일경우
        if (tag == "Player" || tag == "Structures")
        {
            //TODO: 플레이어 일 경우 채집한 광물 -- 진행

            // 충돌 시 자동 반납
            _pool?.Release(this);
        }
    }

    public void OnSpawn()
    {
        //TODO: 꺼내질 때 초기화 처리 등
    }

    public void BulletMove()
    {
        //TODO: 탄막의 이동
    }
}
