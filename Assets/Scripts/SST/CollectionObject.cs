using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectionObject : MonoBehaviour
{
    private CollectionData collectionData;
    private CollectionPool pools;

    [SerializeField] private Canvas sliderCanvas;
    [SerializeField] private Slider slider;

    public CollectionData testCollectionData; //�׽�Ʈ�� �ݷ��ǵ�����
    public CollectionPool testPools; //�׽�Ʈ�� Ǯ

    public bool isTest = false; //�׽�Ʈ ��� �¿���

    public Slider _Slider
    {
        get { return slider; }
        set { slider = value; }
    }


    private float collectTime = 2.5f;                      // ä�� �Ϸ���� �ɸ��� �ð�
    private float currentTime;                      // 
    private bool isCollecting = false;              // ä�� ������ �ƴ��� üũ
    private Transform playerTransform;              // �÷��̾�� �Ÿ� ��� ����


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
        collectionData = data;
        pools = pool;
    }

    // ä�� ���۽� ȣ��Ǵ� �Լ�
    public void StartCollecting(Transform playerPos)
    {
        playerTransform = playerPos;
        isCollecting = true;
        currentTime = 0f;
        sliderCanvas.enabled = true;
    }

    // ä���� ���⶧ ȣ���ϴ� �Լ�
    public void StopCollecting()
    {
        isCollecting = false;
        currentTime = 0f;
        slider.value = 0f;
        sliderCanvas.enabled = false;
    }

    private void Update()
    {
        if (isCollecting)
        {
            float distance = Vector3.Distance(playerTransform.position, transform.position);

            if(distance > 1.5f)
            {
                StopCollecting();
                return;
            }

            currentTime += Time.deltaTime;
            slider.value = currentTime / collectTime;

            if(currentTime >= collectTime)
            {
                CollectCompleted();
            }
        }
    }

    // ä�� �Ϸ����� �� ȣ��Ǵ� �Լ�
    public void CollectCompleted()
    {
        StopCollecting();

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
            pools.ReturnObject(collectionData.collectionName, this.gameObject);
        }
        
    }
        // �� Ǯ�� ��ȯ
        pools.ReturnObject(collectionData.collectionName, this.gameObject); 
        // �� Ȱ��ȭ�� ä�� ����Ʈ���� ����
        FindObjectOfType<CollectionSpawner>().RemoveFromActiveList(this.gameObject);
    }
}
