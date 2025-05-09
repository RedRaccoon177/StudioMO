using UnityEngine;
using UnityEngine.UIElements;

public class BulletSpawner : MonoBehaviour
{
    //@ 발사 간격
    //csv 파일마다 발사할거임.

    [Header("Bullet 풀 매니저")]
    public ObjectPoolingBullet bulletPooling;

    [Header("맵 중앙 기준점")]
    public GameObject mapCenter;

    [Header("발사할 방향 (중앙 기준)")]
    Vector3 fireDirection;

    [Header("이 벽의 콜라이더")]
    public BoxCollider wallCollider;

    [Header("발사 간격 (초)")]
    public float fireInterval = 1f;

    float fireTimer;

    void Start()
    {
        fireDirection = (mapCenter.transform.position - transform.position).normalized;
    }

    void Update()
    {
        CheckFireTimer();
    }

    /// <summary>
    /// GameManager 또는 외부에서 발사 타이머를 제어 가능하도록 분리
    /// </summary>
    public void CheckFireTimer()
    {
        fireTimer += Time.deltaTime;
        if (fireTimer >= fireInterval)
        {
            fireTimer = 0f;
            FireBullet();
        }
    }

    /// <summary>
    /// 벽의 긴 축 방향으로 랜덤 위치에서 총알 생성 후 방향을 중앙 기준 + 각도 편차로 발사
    /// </summary>
    void FireBullet()
    {
        Bounds bounds = wallCollider.bounds;
        Vector3 spawnPos = transform.position;

        // 긴 쪽 축 판단 (X vs Z)
        bool useX = bounds.size.x > bounds.size.z;

        if (useX)
        {
            float randX = Random.Range(bounds.min.x, bounds.max.x);
            spawnPos = new Vector3(randX, transform.position.y, transform.position.z);
        }
        else
        {
            float randZ = Random.Range(bounds.min.z, bounds.max.z);
            spawnPos = new Vector3(transform.position.x, transform.position.y, randZ);
        }

        // 총알 가져오기
        Bullet bullet = bulletPooling.GetBullet();
        bullet.transform.position = spawnPos;

        // 발사 방향 계산 (벽에서 중앙)
        Vector3 fireDir = (mapCenter.transform.position - spawnPos).normalized;

        // 각도 편차 추가 (ex: ±15도)
        float angleOffset = Random.Range(-15f, 15f); // 좌우로 퍼지는 정도
        fireDir = Quaternion.AngleAxis(angleOffset, Vector3.up) * fireDir;

        // 이동 방향 지정
        bullet.Initialize(fireDir.normalized);
    }

}
