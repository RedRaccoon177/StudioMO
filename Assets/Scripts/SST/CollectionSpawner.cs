using Photon.Pun.Demo.PunBasics;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.XR.Interaction.Toolkit;

public class CollectionSpawner : MonoBehaviour
{
    [SerializeField] private CollectionPool poolManager;        // ������Ʈ Ǯ ����
    [SerializeField] private BoxCollider spawnArea;             // ä������ ������ �ڽ� ����

    [SerializeField] private int spawnCount = 5;                // ���� �� ������ ���� ����
    [SerializeField] private float overlapRadius = 0.6f;        // ���� ��ħ ���� ����
    [SerializeField] private int maxSpawnAttempts = 20;         // ��ġ�� �ʰ� �����ϴ� �õ� Ƚ��

    // �� ���� �ʿ� �������� ä���� ����Ʈ ( ä���� ���� Ȯ�� ���� )
    private List<GameObject> activeCollections = new();

    private float spawnInterval = 10f;          // ä���� �ڵ� ���� �ֱ�
    private int maxCountCollection = 20;        // ä���� �ִ� ���� �� ����

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

            // �� �������� ���� ���� ��ŭ �ݺ�
            while (count < spawnCount)
            {
                // �� ��ġ�� �ʴ� ��ġ ã�� ���� ��
                if (TryFindValidPosition(data, out Vector3 pos))
                {
                    var obj = poolManager.GetObject(data);      // Ǯ���� ������Ʈ ��������

                    if (obj != null)
                    {
                        obj.transform.position = pos;       // ��ġ ����
                        obj.transform.SetParent(poolManager.transform, true);   // �θ� ����
                        obj.GetComponent<CollectionObject>().InitializeCollectionObject(data, poolManager);
                        activeCollections.Add(obj);         // Ȱ��ȭ ����Ʈ�� ���
                        count++;
                    }                    
                }
                else
                {
                    Debug.LogWarning($"[�ʱ� ���� ����] {data.collectionName} ��ġ ����");
                    break;      // ��ȿ�� ��ġ ã�� �������� �ߴ�
                }
            }
        }
    }

    // �����ֱ⸶�� �������� ä������ �����ϴ� �ڷ�ƾ
    IEnumerator AutoSpawnRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            // ���� Ȱ��ȭ �Ǿ��ִ� ä���� ���� �ִ� ���� �� ���� ���ٸ�
            if(activeCollections.Count < maxCountCollection)
            {
                // �������� ä���� ������ ���, ���� ��ġ�� �����Ѵ�.
                SpawnOneRandomColleciton();            
            }
        }
    }
    
    //  �������� ä���� ���� �� �ϳ� ���, ���� ��ġ�� �����ϴ� �Լ�
    public void SpawnOneRandomColleciton()
    {
        // �� ������ ����Ʈ ���
        var dataList = poolManager.GetCollectionDataList();

        if(dataList.Count == 0)
        {
            return;
        }

        var data = dataList[Random.Range(0, dataList.Count)];       // ���� ����
        
        // �� ��ȿ�� ��ġ�� ã�� ( ä�������� ��ġ�� �ʴ� )
        if(TryFindValidPosition(data, out Vector3 pos))
        {
            var obj = poolManager.GetObject(data);      // Ǯ���� ������Ʈ ��������

            if (obj == null)
            {
                // Ǯ�� ������ ���� ����� Ǯ�� �߰��� �� �ٽ� ����
                obj = Instantiate(data.collectionPrefab);
                poolManager.ReturnObject(data, obj);
                obj = poolManager.GetObject(data);
            }

            obj.transform.position = pos;                               // ��ġ ����
            obj.transform.SetParent(poolManager.transform, true);       // �θ� ����
            obj.GetComponent<CollectionObject>().InitializeCollectionObject(data, poolManager); //�ʱ�ȭ
            activeCollections.Add(obj);     // Ȱ��ȭ ����Ʈ�� ���
        }
    }

    // �� ä�������� ��ġ�� �ʴ� ��ȿ�� ��ġ�� ã�� �Լ�
    public bool TryFindValidPosition(CollectionData data, out Vector3 result)
    {
        for (int i = 0; i < maxSpawnAttempts; i++)      // �õ� Ƚ����ŭ �ݺ�
        {
            Vector3 pos = GetRandomPositionInBox();     // ���� ��ġ ����

            if(data.collectionType == CollectionType.Special)
            {
                pos.y += 0.3f;      // Ư�� ä���� ��ġ ��¦ �ø�
            }

            // �� �ش� ��ġ�� �̹� �����ϴ� �ٸ� ä������ �ִ��� �˻�
            Collider[] hits = Physics.OverlapSphere(pos, overlapRadius, LayerMask.GetMask("Interactive"));
            if (hits.Length == 0)
            {
                result = pos;       // ��ġ�� �ʴ´ٸ� �ش� ��ġ ��ȯ
                return true;
            }

        }

        result = Vector3.zero;      // ���� �� �⺻�� ��ȯ
        return false;
    }

    // BoxCollider ���� �ȿ��� ���� ��ġ �ϳ� ���ϴ� �Լ�
    Vector3 GetRandomPositionInBox()
    {
        // Boxcollider�� ���� �߽� ��ġ ���
        Vector3 center = spawnArea.center + spawnArea.transform.position;
        Vector3 size = spawnArea.size;

        // X, Z ���� ����. Y���� Raycast�� �ٴ� ��ġ ã��
        float x = Random.Range(center.x - size.x / 2f, center.x + size.x / 2f);
        float z = Random.Range(center.z - size.z / 2f, center.z + size.z / 2f);

        Vector3 rayStart = new Vector3(x, spawnArea.transform.position.y + spawnArea.size.y + 5f, z);
        Ray ray = new Ray(rayStart, Vector3.down);                   // ������ �Ʒ��� Ray ��
        Debug.DrawRay(rayStart, Vector3.down * 100f, Color.red, 3f); // �� �信�� �ð�ȭ

        // Ground ���̾� ��, ���� Ray�� �������
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, LayerMask.GetMask("Ground")))
        {
            Debug.Log($"[Ray ����] Hit: {hit.collider.name}, Y: {hit.point.y}");
            return new Vector3(x, hit.point.y, z);      // ��Ȯ�� ���� ��ȯ
        }

        Debug.LogWarning($"[Ray ����] �ٴ� �� ���� �� �⺻ ��ġ ���");
        return new Vector3(x, center.y, z); // ���� ��ġ�� fallback

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
