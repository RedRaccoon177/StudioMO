using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionPool : MonoBehaviour
{
    // �� ��ũ���ͺ� ������Ʈ ���� ����Ʈ
    [SerializeField] List<CollectionData> collectionDatas;

    // �� �� ä���� �̸����� ť�� ����� ������Ʈ�� ��ųʸ��� ����
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
