using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionPool : MonoBehaviour
{
    // ▼ 스크립터블 오브젝트 담을 리스트
    [SerializeField] List<CollectionData> collectionDatas;

    // ▼ 각 채집 데이터별 오브젝트 풀 저장소
    Dictionary<CollectionData, Queue<GameObject>> pools = new();

    // ▼ 오브젝트 풀 초기화 함수
    public void Initialize()
    {
        // ▼ 모든 채집 데이터 순회
        foreach (var collectionData in collectionDatas)
        {
            // ▼ 각 채집 데이터마다 독립적인 오브젝트 큐 생성
            Queue<GameObject> queue = new();

            for (int i = 0; i < 10; i++)        // 초기 풀 크기 10개
            {
                // ▼ 프리팹으로부터 새 오브젝트 생성
                var obj = Instantiate(collectionData.collectionPrefab, transform);
                obj.SetActive(false);           // 꺼주고
                queue.Enqueue(obj);             // 큐에 넣고
            }

            // ▼ 해당 데이터와 큐를 딕셔너리에 등록
            pools[collectionData] = queue;
        }
    }

    // 풀에서 오브젝트 꺼내는 함수
    public GameObject GetObject(CollectionData data)
    {
        // 해당 데이터의 풀 큐가 존재하고, 큐에 오브젝트가 있다면?
        if (pools.TryGetValue(data, out var queue) && queue.Count > 0)
        {
            GameObject obj = queue.Dequeue();  // 큐에서 하나 꺼내고
            obj.SetActive(true);        // 켜주고
            return obj;                 // 그 오브젝트 반환
        }
        return null;
    }
    
    // 풀에다가 다시 오브젝트를 반환해서 넣어주는 함수
    public void ReturnObject(CollectionData data, GameObject obj)
    {
        obj.SetActive(false);           // 비활성화 처리
        pools[data].Enqueue(obj);       // 큐에 다시 넣기
    }

    public List<CollectionData> GetCollectionDataList()
    {
        return collectionDatas;
    }
}
