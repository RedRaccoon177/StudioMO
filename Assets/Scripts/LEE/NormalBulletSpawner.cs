using UnityEngine;

public class NormalBulletSpawner : MonoBehaviour
{
    #region ź��(�ν�) ������ �ʵ�
    [Header("Bullet Ǯ �Ŵ���")]
    public ObjectPoolingBullet bulletPooling;

    [Header("�߻��� Bullet ������")]
    public NormalBullet normalBulletPrefab;

    [Header("�Ѿ� ������ �θ�� ����� ������Ʈ")]
    public Transform bulletParent;

    [Header("�� �߾� ������")]
    public GameObject mapCenter;

    [Header("�� ���� �ݶ��̴�")]
    public BoxCollider wallCollider;

    [Header("�߻� �� ���� ���� ����")]
    public float plusAngle = 15f;
    public float minusAngle = -15f;

    [Header("BPM ��� ���� (false �� CSV�θ� �߻��)")]
    public bool useAutoFire = true;
    #endregion

    private float fireTimer;

    void Update()
    {
        //if (useAutoFire)
        //{
        //    fireTimer += Time.deltaTime;
        //    if (fireTimer >= 1f) // BPM ����̸� ���õ�
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
