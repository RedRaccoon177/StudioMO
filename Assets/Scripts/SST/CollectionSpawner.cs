using Photon.Pun.Demo.PunBasics;
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

    // ▼ 현재 맵에 존재중인 채집물 리스트 ( 채집물 갯수 확인 위함 )
    private List<GameObject> activeCollections = new List<GameObject>();

    private float spawnInterval = 10f;          // 채집물 자동 생성 주기
    private int maxCountCollection = 20;        // 채집물 최대 생성 수

    private void Start()
    {
        poolManager.Initialize();               // 오브젝트 풀 초기화
        SpawnCollectionInitialize();            // 처음 1회, 채집물 생성
        StartCoroutine(AutoSpawnRoutine());     // 일정 주기마다 채집물 랜덤 생성 
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

    // 일정주기마다 랜덤으로 채집물을 생성하는 코루틴
    IEnumerator AutoSpawnRoutine()
    {
        yield return new WaitForSeconds(spawnInterval);

        // 만약 활성화 되어있는 채집물 수가 최대 제한 수 보다 적다면
        if(activeCollections.Count < maxCountCollection)
        {
            // 랜덤으로 채집물 종류를 골라서, 랜덤 위치에 생성한다.
            SpawnOneRandomColleciton();            
        }
    }
    
    //  랜덤으로 채집물 종류 중 하나 골라서, 랜덤 위치에 생성하는 함수
    public void SpawnOneRandomColleciton()
    {
        var dataList = poolManager.GetCollectionDataList();

        if(dataList.Count == 0)
        {
            return;
        }

        var data = dataList[Random.Range(0, dataList.Count)];

        Vector3 pos = Vector3.zero;
        bool placed = false;

        for (int i = 0; i < maxSpawnAttempts; i++)
        {
            pos = GetRandomPositionInBox();

            if(data.collectionType == CollectionType.Special)
            {
                pos.y += 0.3f;
            }

            // OverlapSphere로 채집물끼리 겹치는지를 체크한다.
            Collider[] hits = Physics.OverlapSphere(pos, overlapRadius, LayerMask.GetMask("Collection"));
            
            // 겹치는게 없을 때 for문 빠져나와서 생성
            if(hits.Length == 0)
            {
                placed = true;
                break;
            }            
        }

        if (!placed)
        {
            Debug.LogError("채집물끼리 겹치지 않는 공간을 찾지 못했습니다.");
            return;
        }

        GameObject obj = poolManager.GetObject(data.collectionName);

        if(obj == null)
        {
            obj = Instantiate(data.collectionPrefab);
            obj.SetActive(false);
            poolManager.ReturnObject(data.collectionName, obj);
            obj = poolManager.GetObject(data.collectionName);            
        }

        obj.transform.position = pos;       // 위치 설정
        obj.transform.SetParent(poolManager.transform, true);       // 부모 설정

        var co = obj.GetComponent<CollectionObject>();
        co.InitializeCollectionObject(data, poolManager);

        activeCollections.Add(obj);
    }

    // 채집 완료시 리스트에서 제거
    public void RemoveFromActiveList(GameObject obj)
    {
        if (activeCollections.Contains(obj))
        {
            activeCollections.Remove(obj);
        }
    }
}
