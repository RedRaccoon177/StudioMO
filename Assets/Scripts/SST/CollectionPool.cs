using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionPool : MonoBehaviour
{
    // �� ��ũ���ͺ� ������Ʈ ���� ����Ʈ
    [SerializeField] List<CollectionData> collectionDatas;

    // �� �� ä�� �����ͺ� ������Ʈ Ǯ �����
    Dictionary<CollectionData, Queue<GameObject>> pools = new();

    // �� ������Ʈ Ǯ �ʱ�ȭ �Լ�
    public void Initialize()
    {
        // �� ��� ä�� ������ ��ȸ
        foreach (var collectionData in collectionDatas)
        {
            // �� �� ä�� �����͸��� �������� ������Ʈ ť ����
            Queue<GameObject> queue = new();

            for (int i = 0; i < 10; i++)        // �ʱ� Ǯ ũ�� 10��
            {
                // �� ���������κ��� �� ������Ʈ ����
                var obj = Instantiate(collectionData.collectionPrefab, transform);
                obj.SetActive(false);           // ���ְ�
                queue.Enqueue(obj);             // ť�� �ְ�
            }

            // �� �ش� �����Ϳ� ť�� ��ųʸ��� ���
            pools[collectionData] = queue;
        }
    }

    // Ǯ���� ������Ʈ ������ �Լ�
    public GameObject GetObject(CollectionData data)
    {
        // �ش� �������� Ǯ ť�� �����ϰ�, ť�� ������Ʈ�� �ִٸ�?
        if (pools.TryGetValue(data, out var queue) && queue.Count > 0)
        {
            GameObject obj = queue.Dequeue();  // ť���� �ϳ� ������
            obj.SetActive(true);        // ���ְ�
            return obj;                 // �� ������Ʈ ��ȯ
        }
        return null;
    }
    
    // Ǯ���ٰ� �ٽ� ������Ʈ�� ��ȯ�ؼ� �־��ִ� �Լ�
    public void ReturnObject(CollectionData data, GameObject obj)
    {
        obj.SetActive(false);           // ��Ȱ��ȭ ó��
        pools[data].Enqueue(obj);       // ť�� �ٽ� �ֱ�
    }

    public List<CollectionData> GetCollectionDataList()
    {
        return collectionDatas;
    }
}
