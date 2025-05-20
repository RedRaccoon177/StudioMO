using UnityEngine;

public class NormalBulletSpawner : MonoBehaviour
{
    #region 탄막(인식) 생성의 필드
    [Header("Bullet 풀 매니저")]
    public ObjectPoolingBullet bulletPooling;

    [Header("발사할 Bullet 프리팹")]
    public NormalBullet normalBulletPrefab;

    [Header("총알 생성시 부모로 사용할 오브젝트")]
    public Transform bulletParent;

    [Header("맵 중앙 기준점")]
    public GameObject mapCenter;

    [Header("이 벽의 콜라이더")]
    public BoxCollider wallCollider;

    [Header("발사 시 각도 범위 지정")]
    public float plusAngle = 15f;
    public float minusAngle = -15f;

    [Header("BPM 기반 여부 (false 시 CSV로만 발사됨)")]
    public bool useAutoFire = true;
    #endregion

    private float fireTimer;

    void Update()
    {
        //if (useAutoFire)
        //{
        //    fireTimer += Time.deltaTime;
        //    if (fireTimer >= 1f) // BPM 기반이면 무시됨
        //    {
        //        fireTimer = 0f;
        //        FireBullet();
        //    }
        //}
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

        NormalBullet bullet = bulletPooling.GetBullet<NormalBullet>();
        bullet.transform.position = spawnPos;

        Vector3 fireDir = (mapCenter.transform.position - spawnPos).normalized;
        float angleOffset = Random.Range(minusAngle, plusAngle);
        fireDir = Quaternion.AngleAxis(angleOffset, Vector3.up) * fireDir;

        bullet.Initialize(fireDir.normalized);
    }
}
