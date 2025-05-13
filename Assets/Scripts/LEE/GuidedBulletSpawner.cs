using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuidedBulletSpawner : MonoBehaviour
{
    #region ź��(�ν�) ������ �ʵ�
    [Header("Bullet Ǯ �Ŵ���")]
    public ObjectPoolingBullet bulletPooling;

    [Header("�߻��� Bullet ������")]
    public GuidedBullet GuidedBulletPrefab;

    [Header("�Ѿ� ������ �θ�� ����� ������Ʈ")]
    public Transform bulletParent;

    [Header("�÷��̾��� ��ġ")]
    //TODO: �ӽ� �÷��̾� ��ġ�� �ʿ��ؼ�
    public TestPlayer player;

    [Header("�߻��� ���� (�÷��̾� ����)")]
    Vector3 fireDirection;

    [Header("�� ���� �ݶ��̴�")]
    public BoxCollider wallCollider;

    [Header("�߻� ���� (��)")]
    public float fireInterval = 3f;

    // �߻� �ֱ⸦ �����ϱ� ���� ���� Ÿ�̸�
    float fireTimer;

    [Header("ź���� �߻� �� ���� ���� ����")]
    public float plusAngle = 0f;
    public float minusAngle = 0f;
    #endregion

    #region Start, Update
    /// <summary>
    /// �ʱ� ����
    /// �߻� ���� ��� �� ź�� Ǯ ����
    /// </summary>
    void Start()
    {
        // �� �߾��� ���� ���� ���͸� ���
        fireDirection = (player.transform.position - transform.position).normalized;

        // �Ѿ� Ǯ�� �����Ͽ� �غ�
        bulletPooling.CreatePool(GuidedBulletPrefab, bulletParent);
    }

    /// <summary>
    /// �� �����Ӹ��� �߻� Ÿ�̸Ӹ� üũ�ϰ�, �߻� Ÿ�̹��� �Ǹ� ź�� �߻�
    /// </summary>
    void Update()
    {
        CheckFireTimer();
    }
    #endregion

    #region �߻� ���� �Լ���
    /// <summary>
    /// �߻� ����(fireInterval)�� ������� ź���� �ֱ������� �߻��Ѵ�.
    /// GameManager �� �ܺο��� ���� ȣ�⵵ �����ϰ� public���� ����
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
    /// ���� ���� ������ ��ġ���� �Ѿ��� ������,
    /// �� �߾� �������� �߻��Ѵ�. �ణ�� ���� ������ �߰��Ͽ� �ڿ������� ���� ȿ���� �ش�.
    /// </summary>
    void FireBullet()
    {
        Bounds bounds = wallCollider.bounds;
        Vector3 spawnPos = transform.position;

        // ���� �� ��(X �Ǵ� Z)�� ���� ���� ��ġ�� ���� ����
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

        // �Ѿ� ������
        GuidedBullet bullet = bulletPooling.GetBullet<GuidedBullet>();
        bullet.transform.position = spawnPos;

        // �߾��� ���� �⺻ ���� ���
        Vector3 fireDir = (player.transform.position - spawnPos).normalized;

        // �¿� ���� ������ �����ϰ� �߰��Ͽ� �۶߸���
        float angleOffset = Random.Range(minusAngle, plusAngle);
        fireDir = Quaternion.AngleAxis(angleOffset, Vector3.up) * fireDir;

        // �Ѿ� �̵� ���� �ʱ�ȭ
        bullet.Initialize(fireDir.normalized);
    }
    #endregion
}