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
}
