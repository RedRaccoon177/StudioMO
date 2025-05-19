using UnityEngine;

public class GuidedBulletSpawner : MonoBehaviour
{
    #region 탄막(인식) 생성의 필드
    [Header("Bullet 풀 매니저")]
    public ObjectPoolingBullet bulletPooling;

    [Header("발사할 Bullet 프리팹")]
    public GuidedBullet guidedBulletPrefab;

    [Header("총알 생성시 부모로 사용할 오브젝트")]
    public Transform bulletParent;

    [Header("플레이어 위치 참조")]
    public GameObject player;

    [Header("이 벽의 콜라이더")]
    public BoxCollider wallCollider;

    [Header("BPM 기반 여부 (false 시 CSV로만 발사됨)")]
    public bool useAutoFire = true;
    #endregion

    private float fireTimer;


    void Update()
    {
        if (useAutoFire)
        {
            fireTimer += Time.deltaTime;
            if (fireTimer >= 1f)
            {
                fireTimer = 0f;
                FireBullet();
            }
        }
    }

    public void FireBullet()
    {
        Bounds bounds = wallCollider.bounds;
        Vector3 spawnPos = transform.position;

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

        GuidedBullet bullet = bulletPooling.GetBullet<GuidedBullet>();
        bullet.transform.position = spawnPos;

        // 정확히 플레이어를 향한 직선 방향
        Vector3 fireDir = (player.transform.position - spawnPos).normalized;

        bullet.Initialize(fireDir);
    }
}
