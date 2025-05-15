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

    [SerializeField] private int spawnCount = 5;                // ���� �� ������ ���� ����
    [SerializeField] private float overlapRadius = 0.6f;        // ���� ��ħ ���� ����
    [SerializeField] private int maxSpawnAttempts = 20;         // ��ġ�� �ʰ� �����ϴ� �õ� Ƚ��

    // �� ���� �ʿ� �������� ä���� ����Ʈ ( ä���� ���� Ȯ�� ���� )
    private List<GameObject> activeCollections = new List<GameObject>();

    private float spawnInterval = 10f;          // ä���� �ڵ� ���� �ֱ�
    private int maxCountCollection = 20;        // ä���� �ִ� ���� ��

    private void Start()
    {
        poolManager.Initialize();               // ������Ʈ Ǯ �ʱ�ȭ
        SpawnCollectionInitialize();            // ó�� 1ȸ, ä���� ����
        StartCoroutine(AutoSpawnRoutine());     // ���� �ֱ⸶�� ä���� ���� ���� 
    }

    // ó���� ä�������� �� �������� ������ ������ŭ ����
    void SpawnCollectionInitialize()
    {
        // ScriptableObject �� ��ϵ� ä���� �����͸� �ϳ��� �ݺ�
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
                    Debug.Log($"[�õ� {i}] �˻� ��ġ: {pos}");
                    Collider[] hits = Physics.OverlapSphere(pos, overlapRadius,layerMask);
                    Debug.Log($"��ħ ���� ����: {hits.Length}");

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
                    // ��ġ �� ã���� �ٽ� Ǯ�� ��ȯ
                    Debug.LogWarning($"[���� ����] {data.collectionName} ��ġ�� �ʴ� ��ġ�� �� ã��");
                    break;
                }

                // ������Ʈ Ǯ���� ������, ��ġ ����
                GameObject obj = poolManager.GetObject(data.collectionName);
                if (obj != null)
                {
                    obj.transform.SetParent(null);             // �θ� �и�
                    obj.transform.position = pos;              // ��ġ ����
                    obj.transform.SetParent(poolManager.transform, true);  // �ٽ� �θ��, ��ġ ����

                    // �ʱ�ȭ
                    var co = obj.GetComponent<CollectionObject>();
                    co.InitializeCollectionObject(data, poolManager);

                    count++;
                }
            }
        }
    }

    // BoxCollider ���� �ȿ��� ���� ��ġ �ϳ� ���ϴ� �Լ�
    Vector3 GetRandomPositionInBox()
    {
        // Boxcollider�� ���� �߽� ��ġ ���
        Vector3 center = spawnArea.center + spawnArea.transform.position;
        Vector3 size = spawnArea.size;

        // X, Z ���� ����. Y���� Ÿ�Կ� ���� �ٸ���
        float x = Random.Range(center.x - size.x / 2f , center.x + size.x / 2f);
        float z = Random.Range(center.z - size.z / 2f, center.z + size.z / 2f);

        Vector3 rayStart = new Vector3(x, spawnArea.transform.position.y + spawnArea.size.y + 5f, z);
        Ray ray = new Ray(rayStart, Vector3.down);
        Debug.DrawRay(rayStart, Vector3.down * 100f, Color.red, 3f); // �� �信�� �ð�ȭ

        int groundLayer = LayerMask.GetMask("Ground");

        if(Physics.Raycast(ray, out RaycastHit hit, 100f, groundLayer))
        {
            Debug.Log($"[Ray ����] Hit: {hit.collider.name}, Y: {hit.point.y}");
            return new Vector3(x, hit.point.y, z);
        }

        Debug.LogWarning($"[Ray ����] �ٴ� �� ���� �� �⺻ ��ġ ���");
        return new Vector3(x, center.y, z); // ���� ��ġ�� fallback

    }

    // �����ֱ⸶�� �������� ä������ �����ϴ� �ڷ�ƾ
    IEnumerator AutoSpawnRoutine()
    {
        yield return new WaitForSeconds(spawnInterval);

        // ���� Ȱ��ȭ �Ǿ��ִ� ä���� ���� �ִ� ���� �� ���� ���ٸ�
        if(activeCollections.Count < maxCountCollection)
        {
            // �������� ä���� ������ ���, ���� ��ġ�� �����Ѵ�.
            SpawnOneRandomColleciton();            
        }
    }
    
    //  �������� ä���� ���� �� �ϳ� ���, ���� ��ġ�� �����ϴ� �Լ�
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

            // OverlapSphere�� ä�������� ��ġ������ üũ�Ѵ�.
            Collider[] hits = Physics.OverlapSphere(pos, overlapRadius, LayerMask.GetMask("Collection"));
            
            // ��ġ�°� ���� �� for�� �������ͼ� ����
            if(hits.Length == 0)
            {
                placed = true;
                break;
            }            
        }

        if (!placed)
        {
            Debug.LogError("ä�������� ��ġ�� �ʴ� ������ ã�� ���߽��ϴ�.");
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

        obj.transform.position = pos;       // ��ġ ����
        obj.transform.SetParent(poolManager.transform, true);       // �θ� ����

        var co = obj.GetComponent<CollectionObject>();
        co.InitializeCollectionObject(data, poolManager);

        activeCollections.Add(obj);
    }

    // ä�� �Ϸ�� ����Ʈ���� ����
    public void RemoveFromActiveList(GameObject obj)
    {
        if (activeCollections.Contains(obj))
        {
            activeCollections.Remove(obj);
        }
    }
}
