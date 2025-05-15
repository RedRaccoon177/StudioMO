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

    private float collectTime = 2.5f;                      // 채집 완료까지 걸리는 시간
    private float currentTime;                      // 
    private bool isCollecting = false;              // 채집 중인지 아닌지 체크
    private Transform playerTransform;              // 플레이어와 거리 계산 위함

    // 채집물 데이터, 풀 값 받아오는 함수
    public void InitializeCollectionObject(CollectionData data, CollectionPool pool)
    {
        collectionData = data;
        pools = pool;
    }

    // 채집 시작시 호출되는 함수
    public void StartCollecting(Transform playerPos)
    {
        playerTransform = playerPos;
        isCollecting = true;
        currentTime = 0f;
        sliderCanvas.enabled = true;
    }

    // 채집을 멈출때 호출하는 함수
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

    // 채집 완료했을 때 호출되는 함수
    public void CollectCompleted()
    {
        StopCollecting();

        // 점수 증가 등 처리 필요
        Debug.Log($"{collectionData.collectionName} 채집 완료!");

        pools.ReturnObject(collectionData.collectionName, this.gameObject);
    }
}
