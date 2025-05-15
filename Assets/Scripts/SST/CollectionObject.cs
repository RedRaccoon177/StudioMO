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

    private float collectTime = 2.5f;                      // ä�� �Ϸ���� �ɸ��� �ð�
    private float currentTime;                      // 
    private bool isCollecting = false;              // ä�� ������ �ƴ��� üũ
    private Transform playerTransform;              // �÷��̾�� �Ÿ� ��� ����

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

        // ���� ���� �� ó�� �ʿ�
        Debug.Log($"{collectionData.collectionName} ä�� �Ϸ�!");

        pools.ReturnObject(collectionData.collectionName, this.gameObject);
    }
}
