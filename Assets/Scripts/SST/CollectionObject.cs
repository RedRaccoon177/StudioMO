using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectionObject : MonoBehaviour
{
    private CollectionData collectionData;          // SO ������
    private CollectionPool pools;                   // �� ������Ʈ ��ȯ�� ������Ʈ Ǯ ����
    private Transform playerTransform;              // �÷��̾�� �Ÿ� ��� ����

    [SerializeField] private CollectionUICtrl ui;   // �����̴� UI ��Ʈ�ѷ� ����

    private float collectTime = 2.5f;               // ä�� �Ϸ���� �ɸ��� �ð�
    private float currentTime;                      // ������� ä���� �ð� ���� ��
    private bool isCollecting = false;              // ä�� ������ �ƴ��� üũ

    public CollectionData testCollectionData; //�׽�Ʈ�� �ݷ��ǵ�����
    public CollectionPool testPools; //�׽�Ʈ�� Ǯ

    public bool isTest = false; //�׽�Ʈ ��� �¿���

    private void Awake() //�׽�Ʈ�� �ڵ�
    {
        if (isTest == true)
        { 
            InitializeCollectionObject(testCollectionData, testPools);
        }
    }

    // ä���� ������, Ǯ �� �޾ƿ��� �Լ�
    public void InitializeCollectionObject(CollectionData data, CollectionPool pool)
    {
        collectionData = data;      // ä�� ������ ����
        pools = pool;               // ������Ʈ Ǯ ����
    }

    // ä�� ���۽� ȣ��Ǵ� �Լ�, ä�� �õ��ϴ� �÷��̾� ��ġ�� ���ڷ� ����
    public void StartCollecting(Transform playerPos)
    {
        playerTransform = playerPos;        // �÷��̾� ��ġ ����
        currentTime = 0f;                   // ä�� �ð� �ʱ�ȭ
        isCollecting = true;                // ä�� ���·� ����
        ui.ShowCollectionSlider();          // ä�� �����̴� UI ǥ��
    }

    // ä���� �ߴ��ϰų� �Ϸ����� �� ȣ���ϴ� �Լ�
    public void StopCollecting()
    {
        isCollecting = false;               // ä�� ���� �ƴ����� ����
        currentTime = 0f;                   // ä�� �ð� �ʱ�ȭ
        ui.SliderValueUpdate(0f);           // �����̴� �� 0���� �ʱ�ȭ
        ui.HideCollectionSlider();          // �����̴� UI �����
    }

    private void Update()
    {
        if (!isCollecting) return;

        // �� �÷��̾�� ������Ʈ ������ �Ÿ��� ���
        float distance = Vector3.Distance(playerTransform.position, transform.position);

        // �� �÷��̾ �ʹ� �־����ٸ�
        if (distance > 1.5f)
        {
            StopCollecting();           // ä�� ���
            return;
        }

        currentTime += Time.deltaTime;  // ä�� �ð� ����
        ui.SliderValueUpdate(currentTime / collectTime);    // �����̴� �� ���� ( ����� ���� )

        // �� ä�� �ð��� �� á�ٸ�
        if (currentTime >= collectTime)
        {
            CollectCompleted();         // ä�� �Ϸ� ó��
        }

    }

    // ä�� �Ϸ����� �� ȣ��Ǵ� �Լ�
    public void CollectCompleted()
    {
        StopCollecting();           // ä�� ���� ���� �� UI �ʱ�ȭ

        if (collectionData == null)
        {
            Debug.LogError("[CollectCompleted] collectionData�� null�Դϴ�!");
            return;
        }

        if (pools == null)
        {
            Debug.Log("[CollectCompleted] pools�� null�Դϴ�!");
        }

        // ���� ���� �� ó�� �ʿ�
        Debug.Log($"{collectionData.collectionName} ä�� �Ϸ�!");

        if(isTest == true)
        {
            Debug.Log($"Test : "+ collectionData.collectionName + " ä�� �Ϸ�!");
        }

        else
        {
            Debug.Log($"{collectionData.collectionName} ä�� �Ϸ�!");
            pools.ReturnObject(collectionData, this.gameObject);
        }
        
        // �� Ǯ�� ���� ������Ʈ ��ȯ
        pools.ReturnObject(collectionData, this.gameObject); 

        // �� Spawner�� �˷���, Ȱ��ȭ�� ä�� ����Ʈ���� ����
        FindObjectOfType<CollectionSpawner>().RemoveFromActiveList(this.gameObject);
    }

    // �ܺο��� ������ ä�� �������� ä�� �׽�Ʈ�� �� �ִ� �Լ�
    public void AddCollectGauge(float amount)
    {
        // ���� ä�� ���� ���� ����
        if (!isCollecting || collectionData == null) return;

        currentTime += (amount / 100f) * collectTime; // ������ ������ŭ �ð� ����
        float ratio = Mathf.Clamp01(currentTime / collectTime);
        ui.SliderValueUpdate(ratio); // UI �ݿ�

        if (currentTime >= collectTime)
        {
            CollectCompleted(); // ä�� �Ϸ�
        }
    }
}


