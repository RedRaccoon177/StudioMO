using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.XR.Interaction.Toolkit;

public class CollectionSpawner : MonoBehaviour
{
    [SerializeField] private CollectionPool poolManager;
    [SerializeField] private BoxCollider spawnArea;

    [SerializeField] private int spawnCount = 5;                // 시작 시 종류별 생성 갯수
    [SerializeField] private float overlapRadius = 0.6f;        // 생성 겹침 감지 범위
    [SerializeField] private int maxSpawnAttempts = 20;         // 겹치지 않게 생성하는 시도 횟수

    private void Start()
    {
        //spawnArea.enabled = false;
        poolManager.Initialize();           // 오브젝트 풀 초기화
        SpawnCollectionInitialize();        // 처음 1회, 채집물 생성
    }

    // 처음에 채집물들을 각 종류별로 지정된 갯수만큼 생성
    void SpawnCollectionInitialize()
    {
        // ScriptableObject 에 등록된 채집물 데이터를 하나씩 반복
        foreach(var data in poolManager.GetCollectionDataList())
        {
            int count = 0;

            while(count < spawnCount)
            {
                Vector3 pos = Vector3.zero;
                bool placed = false;

                for (int i = 0; i < maxSpawnAttempts; i++)
                {
                    pos = GetRandomPositionInBox();

                    if(data.collectionType == CollectionType.Special)
                    {
                        pos.y += 0.3f;
                    }

                    int layerMask = LayerMask.GetMask("Collection");
                    Debug.Log($"[시도 {i}] 검사 위치: {pos}");
                    Collider[] hits = Physics.OverlapSphere(pos, overlapRadius,layerMask);
                    Debug.Log($"겹침 감지 개수: {hits.Length}");

                    foreach(var h in hits)
                    {
                        Debug.Log($"Hit: {h.name}");
                    }

                    if (hits.Length == 0)
                    {
                        placed = true;
                        break;
                    }
                }

                if (!placed)
                {
                    // 위치 못 찾으면 다시 풀에 반환
                    Debug.LogWarning($"[스폰 실패] {data.collectionName} 겹치지 않는 위치를 못 찾음");
                    break;
                }

                // 오브젝트 풀에서 꺼내고, 위치 설정
                GameObject obj = poolManager.GetObject(data.collectionName);
                if (obj != null)
                {
                    obj.transform.SetParent(null);             // 부모 분리
                    obj.transform.position = pos;              // 위치 설정
                    obj.transform.SetParent(poolManager.transform, true);  // 다시 부모로, 위치 유지

                    // 초기화
                    var co = obj.GetComponent<CollectionObject>();
                    co.InitializeCollectionObject(data, poolManager);

                    count++;
                }
            }
        }
    }

    // BoxCollider 범위 안에서 랜덤 위치 하나 구하는 함수
    Vector3 GetRandomPositionInBox()
    {
        // Boxcollider의 실제 중심 위치 계산
        Vector3 center = spawnArea.center + spawnArea.transform.position;
        Vector3 size = spawnArea.size;

        // X, Z 축은 랜덤. Y축은 타입에 따라 다르게
        float x = Random.Range(center.x - size.x / 2f , center.x + size.x / 2f);
        float z = Random.Range(center.z - size.z / 2f, center.z + size.z / 2f);

        Vector3 rayStart = new Vector3(x, spawnArea.transform.position.y + spawnArea.size.y + 5f, z);
        Ray ray = new Ray(rayStart, Vector3.down);
        Debug.DrawRay(rayStart, Vector3.down * 100f, Color.red, 3f); // 씬 뷰에서 시각화

        int groundLayer = LayerMask.GetMask("Ground");

        if(Physics.Raycast(ray, out RaycastHit hit, 100f, groundLayer))
        {
            Debug.Log($"[Ray 성공] Hit: {hit.collider.name}, Y: {hit.point.y}");
            return new Vector3(x, hit.point.y, z);
        }

        Debug.LogWarning($"[Ray 실패] 바닥 못 맞춤 → 기본 위치 사용");
        return new Vector3(x, center.y, z); // 공중 위치로 fallback

    }
}
