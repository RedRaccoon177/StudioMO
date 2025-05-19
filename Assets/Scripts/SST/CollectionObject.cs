using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectionObject : MonoBehaviour
{
    private CollectionData collectionData;          // SO 참조용
    private CollectionPool pools;                   // 이 오브젝트 반환할 오브젝트 풀 참조
    private Transform playerTransform;              // 플레이어와 거리 계산 위함

    [SerializeField] private CollectionUICtrl ui;   // 슬라이더 UI 컨트롤러 참조

    private float collectTime = 2.5f;               // 채집 완료까지 걸리는 시간
    private float currentTime;                      // 현재까지 채집된 시간 누적 값
    private bool isCollecting = false;              // 채집 중인지 아닌지 체크

    public CollectionData testCollectionData; //테스트용 콜렉션데이터
    public CollectionPool testPools; //테스트용 풀

    public bool isTest = false; //테스트 모드 온오프

    private void Awake() //테스트용 코드
    {
        if (isTest == true)
        { 
            InitializeCollectionObject(testCollectionData, testPools);
        }
    }

    // 채집물 데이터, 풀 값 받아오는 함수
    public void InitializeCollectionObject(CollectionData data, CollectionPool pool)
    {
        collectionData = data;      // 채집 데이터 설정
        pools = pool;               // 오브젝트 풀 설정
    }

    // 채집 시작시 호출되는 함수, 채집 시도하는 플레이어 위치를 인자로 받음
    public void StartCollecting(Transform playerPos)
    {
        playerTransform = playerPos;        // 플레이어 위치 저장
        currentTime = 0f;                   // 채집 시간 초기화
        isCollecting = true;                // 채집 상태로 변경
        ui.ShowCollectionSlider();          // 채집 슬라이더 UI 표시
    }

    // 채집을 중단하거나 완료했을 때 호출하는 함수
    public void StopCollecting()
    {
        isCollecting = false;               // 채집 상태 아님으로 변경
        currentTime = 0f;                   // 채집 시간 초기화
        ui.SliderValueUpdate(0f);           // 슬라이더 값 0으로 초기화
        ui.HideCollectionSlider();          // 슬라이더 UI 숨기기
    }

    private void Update()
    {
        if (!isCollecting) return;

        // ▼ 플레이어와 오브젝트 사이의 거리를 계산
        float distance = Vector3.Distance(playerTransform.position, transform.position);

        // ▼ 플레이어가 너무 멀어졌다면
        if (distance > 1.5f)
        {
            StopCollecting();           // 채집 취소
            return;
        }

        currentTime += Time.deltaTime;  // 채집 시간 증가
        ui.SliderValueUpdate(currentTime / collectTime);    // 슬라이더 값 갱신 ( 진행률 비율 )

        // ▼ 채집 시간이 다 찼다면
        if (currentTime >= collectTime)
        {
            CollectCompleted();         // 채집 완료 처리
        }

    }

    // 채집 완료했을 때 호출되는 함수
    public void CollectCompleted()
    {
        StopCollecting();           // 채집 상태 종료 및 UI 초기화

        if (collectionData == null)
        {
            Debug.LogError("[CollectCompleted] collectionData가 null입니다!");
            return;
        }

        if (pools == null)
        {
            Debug.Log("[CollectCompleted] pools가 null입니다!");
        }

        // 점수 증가 등 처리 필요
        Debug.Log($"{collectionData.collectionName} 채집 완료!");

        if(isTest == true)
        {
            Debug.Log($"Test : "+ collectionData.collectionName + " 채집 완료!");
        }

        else
        {
            Debug.Log($"{collectionData.collectionName} 채집 완료!");
            pools.ReturnObject(collectionData, this.gameObject);
        }
        
        // ▼ 풀에 현재 오브젝트 반환
        pools.ReturnObject(collectionData, this.gameObject); 

        // ▼ Spawner에 알려서, 활성화된 채집 리스트에서 제거
        FindObjectOfType<CollectionSpawner>().RemoveFromActiveList(this.gameObject);
    }

    // 외부에서 강제로 채집 게이지를 채워 테스트할 수 있는 함수
    public void AddCollectGauge(float amount)
    {
        // 실제 채집 중일 때만 진행
        if (!isCollecting || collectionData == null) return;

        currentTime += (amount / 100f) * collectTime; // 게이지 비율만큼 시간 누적
        float ratio = Mathf.Clamp01(currentTime / collectTime);
        ui.SliderValueUpdate(ratio); // UI 반영

        if (currentTime >= collectTime)
        {
            CollectCompleted(); // 채집 완료
        }
    }
}


