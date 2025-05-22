using UnityEngine;
using UnityEngine.Pool;

public class GuidedBullet : MonoBehaviour, IBullet
{
    #region 탄막(인식) 필드
    IObjectPool<GuidedBullet> _guidedBulletPool;
    Vector3 moveDirection;
    public float speed = 3f;
    GameObject currentIndicatorInstance;
    #endregion

    public void SetPool<T>(IObjectPool<T> pool) where T : Component
    {
        _guidedBulletPool = pool as IObjectPool<GuidedBullet>;
    }

    void Update()
    {
        transform.position += moveDirection * speed * Time.deltaTime;

        // 인디케이터 위치 갱신
        if (currentIndicatorInstance != null)
        {
            UpdateIndicator();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var player = other.GetComponentInParent<PlayerController>();
            if (!player.isGroggyAndinvincibleState)
            {
                player.HitBullet();
                _guidedBulletPool?.Release(this);
            }
        }
        else if (other.CompareTag("Structures"))
        {
            _guidedBulletPool?.Release(this);
        }
    }

    void OnDisable()
    {
        // 사라짐 이펙트 출력
        EffectPoolManager.Instance.SpawnEffect("VFX_MON001_Explode", transform.position, Quaternion.identity);

        // 인디케이터 반납
        if (currentIndicatorInstance != null)
        {
            EffectPoolManager.Instance.ReleaseEffect("MON002_Indicator", currentIndicatorInstance);
            currentIndicatorInstance = null;
        }
    }

    public void OnSpawn()
    {
        // 기존 인디케이터 정리
        if (currentIndicatorInstance != null)
        {
            EffectPoolManager.Instance.ReleaseEffect("MON002_Indicator", currentIndicatorInstance);
            currentIndicatorInstance = null;
        }

        // 인디케이터 새로 꺼내서 위치 초기화
        currentIndicatorInstance = EffectPoolManager.Instance.GetEffect("MON002_Indicator");
        if (currentIndicatorInstance != null)
        {
            currentIndicatorInstance.transform.SetPositionAndRotation(transform.position, Quaternion.identity);
            UpdateIndicator();
        }
    }

    public void Initialize(Vector3 direction)
    {
        moveDirection = direction.normalized;
    }

    void UpdateIndicator()
    {
        if (currentIndicatorInstance == null) return;

        var lr = currentIndicatorInstance.GetComponent<LineRenderer>();
        if (lr != null)
        {
            lr.SetPosition(0, transform.position);
            lr.SetPosition(1, transform.position + moveDirection.normalized * 2f);
        }

        currentIndicatorInstance.transform.position = transform.position;
        currentIndicatorInstance.transform.rotation = Quaternion.LookRotation(moveDirection);
    }
}
