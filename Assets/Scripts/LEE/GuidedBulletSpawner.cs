using UnityEngine;

public class GuidedBulletSpawner : MonoBehaviour
{
    #region ź��(�ν�) ������ �ʵ�
    [Header("Bullet Ǯ �Ŵ���")]
    public ObjectPoolingBullet bulletPooling;

    [Header("�߻��� Bullet ������")]
    public GuidedBullet guidedBulletPrefab;

    [Header("�Ѿ� ������ �θ�� ����� ������Ʈ")]
    public Transform bulletParent;

    [Header("�÷��̾� ��ġ ����")]
    public GameObject player;

    [Header("�� ���� �ݶ��̴�")]
    public BoxCollider wallCollider;

    [Header("BPM ��� ���� (false �� CSV�θ� �߻��)")]
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

        // ��Ȯ�� �÷��̾ ���� ���� ����
        Vector3 fireDir = (player.transform.position - spawnPos).normalized;

        bullet.Initialize(fireDir);
    }
}
