using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuidedBulletSpawner : MonoBehaviour
{
    #region 탄막(인식) 생성의 필드
    [Header("Bullet 풀 매니저")]
    public ObjectPoolingBullet bulletPooling;

    [Header("발사할 Bullet 프리팹")]
    public GuidedBullet GuidedBulletPrefab;

    [Header("총알 생성시 부모로 사용할 오브젝트")]
    public Transform bulletParent;

    [Header("플레이어의 위치")]
    //TODO: 임시 플레이어 위치값 필요해서
    public TestPlayer player;

    [Header("발사할 방향 (플레이어 방향)")]
    Vector3 fireDirection;

    [Header("이 벽의 콜라이더")]
    public BoxCollider wallCollider;

    [Header("발사 간격 (초)")]
    public float fireInterval = 3f;

    // 발사 주기를 제어하기 위한 내부 타이머
    float fireTimer;

    [Header("탄막의 발사 시 각도 범위 지정")]
    public float plusAngle = 0f;
    public float minusAngle = 0f;
    #endregion

    #region Start, Update
    /// <summary>
    /// 초기 설정
    /// 발사 방향 계산 및 탄막 풀 생성
    /// </summary>
    void Start()
    {
        // 맵 중앙을 향한 방향 벡터를 계산
        fireDirection = (player.transform.position - transform.position).normalized;

        // 총알 풀을 생성하여 준비
        bulletPooling.CreatePool(GuidedBulletPrefab, bulletParent);
    }

    /// <summary>
    /// 매 프레임마다 발사 타이머를 체크하고, 발사 타이밍이 되면 탄막 발사
    /// </summary>
    void Update()
    {
        CheckFireTimer();
    }
    #endregion

    #region 발사 관련 함수들
    /// <summary>
    /// 발사 간격(fireInterval)을 기반으로 탄막을 주기적으로 발사한다.
    /// GameManager 등 외부에서 수동 호출도 가능하게 public으로 설정
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
    /// 벽을 따라 랜덤한 위치에서 총알을 꺼내고,
    /// 맵 중앙 방향으로 발사한다. 약간의 각도 편차를 추가하여 자연스러운 퍼짐 효과를 준다.
    /// </summary>
    void FireBullet()
    {
        Bounds bounds = wallCollider.bounds;
        Vector3 spawnPos = transform.position;

        // 벽의 긴 축(X 또는 Z)을 따라 스폰 위치를 랜덤 설정
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

        // 총알 꺼내기
        GuidedBullet bullet = bulletPooling.GetBullet<GuidedBullet>();
        bullet.transform.position = spawnPos;

        // 중앙을 향한 기본 방향 계산
        Vector3 fireDir = (player.transform.position - spawnPos).normalized;

        // 좌우 각도 편차를 랜덤하게 추가하여 퍼뜨린다
        float angleOffset = Random.Range(minusAngle, plusAngle);
        fireDir = Quaternion.AngleAxis(angleOffset, Vector3.up) * fireDir;

        // 총알 이동 방향 초기화
        bullet.Initialize(fireDir.normalized);
    }
    #endregion
}