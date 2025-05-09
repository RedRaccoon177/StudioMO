using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionPool : MonoBehaviour
{
    // ▼ 스크립터블 오브젝트 담을 리스트
    [SerializeField] List<CollectionData> collectionDatas;

    // ▼ 각 채집물 이름별로 큐를 만들어 오브젝트를 딕셔너리에 저장
    Dictionary<string, Queue<GameObject>> pools = new Dictionary<string, Queue<GameObject>>();

    private void Awake()
    {
        Queue<GameObject> collectionDataQueue;

        foreach (var collectionData in collectionDatas)
        {
            collectionDataQueue = new Queue<GameObject>();

            for (int i = 0; i < 10; i++)
            {
                var obj = Instantiate(collectionData.collectionPrefab);
                obj.SetActive(false);
                obj.transform.parent = this.transform;
                collectionDataQueue.Enqueue(obj);
            }

            pools.Add(collectionData.collectionName, collectionDataQueue);
        }
    }

    // 풀에서 오브젝트 꺼내는 함수
    public GameObject GetObject(string collectionName)
    {
        // 풀에 받아온 오브젝트와 같은 이름이 있으면서, 해당 오브젝트가 있다면
        if (pools.ContainsKey(collectionName) && pools[collectionName].Count > 0)
        {
            GameObject obj = pools[collectionName].Dequeue();  // 풀에서 해당 오브젝트 빼와서 담고
            obj.SetActive(true);        // 켜주고
            return obj;                 // 그 오브젝트 반환
        }
        return null;
    }
    
    // 풀에다가 다시 오브젝트를 반환해서 넣어주는 함수
    public void ReturnObject(string collectionName, GameObject obj)
    {
        obj.SetActive(false);
        pools[collectionName].Enqueue(obj);
    }
}
