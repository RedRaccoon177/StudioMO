using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// ź��(Bullet)�� �ֱ������� �����ϴ� ���� �Ŵ���
/// �߻� ����, ����, ���� ��ġ ���� �����ϸ�,
/// ObjectPoolingBullet �Ŵ����� ���� �Ѿ��� ������ �߻��Ѵ�.
/// </summary>
public class NormalBulletSpawner : MonoBehaviour
{
    #region ź��(���ν�) ������ �ʵ�
    [Header("Bullet Ǯ �Ŵ���")]
    public ObjectPoolingBullet bulletPooling;

    [Header("�߻��� Bullet ������")]
    public NormalBullet normalBulletPrefab;

    [Header("�Ѿ� ������ �θ�� ����� ������Ʈ")]
    public Transform bulletParent;

    [Header("�� �߾� ������")]
    public GameObject mapCenter;

    [Header("�߻��� ���� (�߾� ����)")]
    Vector3 fireDirection;

    [Header("�� ���� �ݶ��̴�")]
    public BoxCollider wallCollider;

    [Header("�߻� ���� (��)")]
    public float fireInterval = 1f;

    // �߻� �ֱ⸦ �����ϱ� ���� ���� Ÿ�̸�
    float fireTimer;

    [Header("ź���� �߻� �� ���� ���� ����")]
    public float plusAngle = 15f;
    public float minusAngle = -15f;
    #endregion

    #region Start, Update
    /// <summary>
    /// �ʱ� ����
    /// �߻� ���� ��� �� ź�� Ǯ ����
    /// </summary>
    void Start()
    {
        // �� �߾��� ���� ���� ���͸� ���
        fireDirection = (mapCenter.transform.position - transform.position).normalized;

        // �Ѿ� Ǯ�� �����Ͽ� �غ�
        bulletPooling.CreatePool(normalBulletPrefab, bulletParent);
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
        NormalBullet bullet = bulletPooling.GetBullet<NormalBullet>();
        bullet.transform.position = spawnPos;

        // �߾��� ���� �⺻ ���� ���
        Vector3 fireDir = (mapCenter.transform.position - spawnPos).normalized;

        // �¿� ���� ������ �����ϰ� �߰��Ͽ� �۶߸���
        float angleOffset = Random.Range(minusAngle, plusAngle);
        fireDir = Quaternion.AngleAxis(angleOffset, Vector3.up) * fireDir;

        // �Ѿ� �̵� ���� �ʱ�ȭ
        bullet.Initialize(fireDir.normalized);
    }
    #endregion
}
