using UnityEngine;
using UnityEngine.UIElements;

public class BulletSpawner : MonoBehaviour
{
    //@ �߻� ����
    //csv ���ϸ��� �߻��Ұ���.

    [Header("Bullet Ǯ �Ŵ���")]
    public ObjectPoolingBullet bulletPooling;

    [Header("�� �߾� ������")]
    public GameObject mapCenter;

    [Header("�߻��� ���� (�߾� ����)")]
    Vector3 fireDirection;

    [Header("�� ���� �ݶ��̴�")]
    public BoxCollider wallCollider;

    [Header("�߻� ���� (��)")]
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
    /// GameManager �Ǵ� �ܺο��� �߻� Ÿ�̸Ӹ� ���� �����ϵ��� �и�
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
    /// ���� �� �� �������� ���� ��ġ���� �Ѿ� ���� �� ������ �߾� ���� + ���� ������ �߻�
    /// </summary>
    void FireBullet()
    {
        Bounds bounds = wallCollider.bounds;
        Vector3 spawnPos = transform.position;

        // �� �� �� �Ǵ� (X vs Z)
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

        // �Ѿ� ��������
        Bullet bullet = bulletPooling.GetBullet();
        bullet.transform.position = spawnPos;

        // �߻� ���� ��� (������ �߾�)
        Vector3 fireDir = (mapCenter.transform.position - spawnPos).normalized;

        // ���� ���� �߰� (ex: ��15��)
        float angleOffset = Random.Range(-15f, 15f); // �¿�� ������ ����
        fireDir = Quaternion.AngleAxis(angleOffset, Vector3.up) * fireDir;

        // �̵� ���� ����
        bullet.Initialize(fireDir.normalized);
    }

}
