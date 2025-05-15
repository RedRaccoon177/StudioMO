using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionPool : MonoBehaviour
{
    // �� ��ũ���ͺ� ������Ʈ ���� ����Ʈ
    [SerializeField] List<CollectionData> collectionDatas;

    // �� �� ä���� �̸����� ť�� ����� ������Ʈ�� ��ųʸ��� ����
    Dictionary<string, Queue<GameObject>> pools = new Dictionary<string, Queue<GameObject>>();

    public void Initialize()
    {
        // �� �̸� �ߺ� ī��Ʈ�� ��ųʸ�
        Dictionary<string, int> nameCount = new Dictionary<string, int>();

        Queue<GameObject> collectionDataQueue;

        foreach (var collectionData in collectionDatas)
        {
            string baseName = collectionData.collectionName;

            // �̸� �ߺ� �˻� : �̸��� ó�� ���°Ÿ� �� 0���� ���
            if (!nameCount.ContainsKey(baseName))
            {
                nameCount[baseName] = 0;
            }
            else
            {
                nameCount[baseName]++;  // ó�� �ƴ϶�� ���� ����
            }

            string uniqueName = baseName;

            if (nameCount[baseName] > 0)
            {
                uniqueName = baseName + "_" + nameCount[baseName];
            }

            collectionData.collectionName = uniqueName;

            collectionDataQueue = new Queue<GameObject>();

            for (int i = 0; i < 10; i++)
            {
                var obj = Instantiate(collectionData.collectionPrefab);
                obj.SetActive(false);
                obj.transform.parent = this.transform;
                collectionDataQueue.Enqueue(obj);
            }

            pools.Add(uniqueName, collectionDataQueue);
        }
    }

    // Ǯ���� ������Ʈ ������ �Լ�
    public GameObject GetObject(string collectionName)
    {
        // Ǯ�� �޾ƿ� ������Ʈ�� ���� �̸��� �����鼭, �ش� ������Ʈ�� �ִٸ�
        if (pools.ContainsKey(collectionName) && pools[collectionName].Count > 0)
        {
            GameObject obj = pools[collectionName].Dequeue();  // Ǯ���� �ش� ������Ʈ ���ͼ� ���
            obj.SetActive(true);        // ���ְ�
            return obj;                 // �� ������Ʈ ��ȯ
        }
        return null;
    }
    
    // Ǯ���ٰ� �ٽ� ������Ʈ�� ��ȯ�ؼ� �־��ִ� �Լ�
    public void ReturnObject(string collectionName, GameObject obj)
    {
        obj.SetActive(false);
        pools[collectionName].Enqueue(obj);
    }

    public List<CollectionData> GetCollectionDataList()
    {
        return collectionDatas;
    }
}
