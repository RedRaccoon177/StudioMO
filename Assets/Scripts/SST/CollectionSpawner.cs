using Photon.Pun.Demo.PunBasics;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.XR.Interaction.Toolkit;

public class CollectionSpawner : MonoBehaviour
{
    [SerializeField] private CollectionPool poolManager;        // 오브젝트 풀 참조
    [SerializeField] private BoxCollider spawnArea;             // 채집물을 생성할 박스 범위

    [SerializeField] private int spawnCount = 5;                // 시작 시 종류별 생성 갯수
    [SerializeField] private float overlapRadius = 0.6f;        // 생성 겹침 감지 범위
    [SerializeField] private int maxSpawnAttempts = 20;         // 겹치지 않게 생성하는 시도 횟수

    // ▼ 현재 맵에 존재중인 채집물 리스트 ( 채집물 갯수 확인 위함 )
    private List<GameObject> activeCollections = new();

    private float spawnInterval = 10f;          // 채집물 자동 생성 주기
    private int maxCountCollection = 20;        // 채집물 최대 생성 수 제한

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

            // ▼ 종류별로 지정 갯수 만큼 반복
            while (count < spawnCount)
            {
                // ▼ 겹치지 않는 위치 찾기 성공 시
                if (TryFindValidPosition(data, out Vector3 pos))
                {
                    var obj = poolManager.GetObject(data);      // 풀에서 오브젝트 가져오기

                    if (obj != null)
                    {
                        obj.transform.position = pos;       // 위치 지정
                        obj.transform.SetParent(poolManager.transform, true);   // 부모 설정
                        obj.GetComponent<CollectionObject>().InitializeCollectionObject(data, poolManager);
                        activeCollections.Add(obj);         // 활성화 리스트에 등록
                        count++;
                    }                    
                }
                else
                {
                    Debug.LogWarning($"[초기 스폰 실패] {data.collectionName} 배치 실패");
                    break;      // 유효한 위치 찾지 못했으면 중단
                }
            }
        }
    }

    // 일정주기마다 랜덤으로 채집물을 생성하는 코루틴
    IEnumerator AutoSpawnRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            // 만약 활성화 되어있는 채집물 수가 최대 제한 수 보다 적다면
            if(activeCollections.Count < maxCountCollection)
            {
                // 랜덤으로 채집물 종류를 골라서, 랜덤 위치에 생성한다.
                SpawnOneRandomColleciton();            
            }
        }
    }
    
    //  랜덤으로 채집물 종류 중 하나 골라서, 랜덤 위치에 생성하는 함수
    public void SpawnOneRandomColleciton()
    {
        // ▼ 데이터 리스트 얻기
        var dataList = poolManager.GetCollectionDataList();

        if(dataList.Count == 0)
        {
            return;
        }

        var data = dataList[Random.Range(0, dataList.Count)];       // 랜덤 선택
        
        // ▼ 유효한 위치를 찾기 ( 채집물들이 겹치지 않는 )
        if(TryFindValidPosition(data, out Vector3 pos))
        {
            var obj = poolManager.GetObject(data);      // 풀에서 오브젝트 가져오기

            if (obj == null)
            {
                // 풀에 없으면 새로 만들고 풀에 추가한 후 다시 꺼냄
                obj = Instantiate(data.collectionPrefab);
                poolManager.ReturnObject(data, obj);
                obj = poolManager.GetObject(data);
            }

            obj.transform.position = pos;                               // 위치 지정
            obj.transform.SetParent(poolManager.transform, true);       // 부모 설정
            obj.GetComponent<CollectionObject>().InitializeCollectionObject(data, poolManager); //초기화
            activeCollections.Add(obj);     // 활성화 리스트에 등록
        }
    }

    // ▼ 채집물끼리 겹치지 않는 유효한 위치를 찾는 함수
    public bool TryFindValidPosition(CollectionData data, out Vector3 result)
    {
        for (int i = 0; i < maxSpawnAttempts; i++)      // 시도 횟수만큼 반복
        {
            Vector3 pos = GetRandomPositionInBox();     // 랜덤 위치 생성

            if(data.collectionType == CollectionType.Special)
            {
                pos.y += 0.3f;      // 특수 채집물 위치 살짝 올림
            }

            // ▼ 해당 위치에 이미 존재하는 다른 채집물이 있는지 검사
            Collider[] hits = Physics.OverlapSphere(pos, overlapRadius, LayerMask.GetMask("Interactive"));
            if (hits.Length == 0)
            {
                result = pos;       // 겹치지 않는다면 해당 위치 반환
                return true;
            }

        }

        result = Vector3.zero;      // 실패 시 기본값 반환
        return false;
    }

    // BoxCollider 범위 안에서 랜덤 위치 하나 구하는 함수
    Vector3 GetRandomPositionInBox()
    {
        // Boxcollider의 실제 중심 위치 계산
        Vector3 center = spawnArea.center + spawnArea.transform.position;
        Vector3 size = spawnArea.size;

        // X, Z 축은 랜덤. Y축은 Raycast로 바닥 위치 찾기
        float x = Random.Range(center.x - size.x / 2f, center.x + size.x / 2f);
        float z = Random.Range(center.z - size.z / 2f, center.z + size.z / 2f);

        Vector3 rayStart = new Vector3(x, spawnArea.transform.position.y + spawnArea.size.y + 5f, z);
        Ray ray = new Ray(rayStart, Vector3.down);                   // 위에서 아래로 Ray 쏨
        Debug.DrawRay(rayStart, Vector3.down * 100f, Color.red, 3f); // 씬 뷰에서 시각화

        // Ground 레이어 즉, 땅에 Ray가 닿았으면
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, LayerMask.GetMask("Ground")))
        {
            Debug.Log($"[Ray 성공] Hit: {hit.collider.name}, Y: {hit.point.y}");
            return new Vector3(x, hit.point.y, z);      // 정확한 높이 반환
        }

        Debug.LogWarning($"[Ray 실패] 바닥 못 맞춤 → 기본 위치 사용");
        return new Vector3(x, center.y, z); // 공중 위치로 fallback

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
