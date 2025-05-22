using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CollectionObject : MonoBehaviour
{
    private CollectionData collectionData;          // SO 참조용
    private CollectionPool pools;                   // 이 오브젝트 반환할 오브젝트 풀 참조
    private CollectionSpawner spawner;

    private Transform playerTransform;              // 플레이어와 거리 계산 위함

    [SerializeField] private Collider selfColider;  // 채집 플레이어 상호작용 체크 콜라이더
    [SerializeField] private CollectionUICtrl ui;   // 슬라이더 UI 컨트롤러 참조

    private float currentGage = 0f;                 // 채집 슬라이더 게이지 0 ~ 100
    private float coolTime = 0.5f;                  // 채집 콜라이더 히트 쿨타임
    private float gagePerHit = 10f;                 // 차게 될 게이지 값

    private bool isCoolTime = false;                // 쿨타임 체크용
    
    public bool IsCollecting { get { return isCollecting;}}

    private bool isCollecting = false;              // 현재 채집 중인지 아닌지 체크
    private bool isCollected = false;               // 채집 완료 여부 체크

    // 채집물 데이터, 풀 값 받아오는 함수
    public void InitializeCollectionObject(CollectionData data, CollectionPool pool, CollectionSpawner collectionSpawner)
    {
        collectionData = data;      // 채집 데이터 설정
        pools = pool;               // 오브젝트 풀 설정
        spawner = collectionSpawner;
    }

    // 채집 시작시 호출되는 함수, 채집 시도하는 플레이어 위치를 인자로 받음
    public void StartCollecting(Transform playerPos)
    {
        isCollecting = true;                // 채집중인 상태로 변경
        isCollected = false;                // 채집 완료 초기화
        currentGage = 0f;                   // 채집 게이지 0으로 초기화
        ui.ShowCollectionSlider();          // 채집 슬라이더 UI 표시
        Debug.Log("채집이 시작 되었습니다.");
    }

    // ▼ 채집물 슬라이더 값 증가 시도하는 함수 - 실제 게이지 누적하고 쿨타임 처리
    public void TryAddCollectGage(float value)
    {
        // ▼ 이미 채집이 완료 되었거나, 쿨타임 중이거나, 채집 상태가 아니라면 무시
        if (isCollected || isCoolTime || !isCollecting) return;

        isCoolTime = true;                  // 쿨타임 시작

        selfColider.enabled = false;        // 콜라이더 잠깐 꺼서 중복 히트 방지

        // ▼ 게이지 증가 및 채집 완료 판단은 코루틴에서 하도록 바꿈
        StartCoroutine(TryCollectGageCoroutine(value));
    }

    // 외부에서 강제로 채집 게이지를 채워 테스트할 수 있는 함수
    public void AddCollectGauge(float amount)
    {
        currentGage += amount;                                  // 게이지 누적
        currentGage = Mathf.Clamp(currentGage, 0f, 100f);       // 0 ~ 100 으로 제한두고

        ui.SliderValueUpdate(currentGage);                      // 슬라이더 값 업데이트

        if (currentGage >= 100f)    // 슬라이더 게이지 값 100하고 같거나 넘어가면
        {
            CollectCompleted();     // 채집 완료
        }
    }

    // 채집 완료했을 때 호출되는 함수
    public void CollectCompleted()
    {
        isCollected = true;             // 채집 완료 상태로 변경
        isCollecting = false;           // 채집 중 상태 아님
        ui.HideCollectionSlider();      // 슬라이더 UI 숨김

        // 점수 증가 등 처리 필요
        Debug.Log($"{collectionData.collectionName} 채집 완료!");
       
        // ▼ 풀에 현재 오브젝트 반환
        pools.ReturnObject(collectionData, this.gameObject); 

        // ▼ Spawner에 알려서, 활성화된 채집 리스트에서 제거
        spawner.RemoveFromActiveList(this.gameObject);
    }

    // 게이지 증가 및 채집 완료 판단은 코루틴에서
    IEnumerator TryCollectGageCoroutine(float value)
    {
        AddCollectGauge(value);                         // 여기서 게이지 증가 및 채집 완료 여부 체크

        // ▼ 채집 완료됬다면, 오브젝트 풀에 반환되고 비활성화되므로, 코루틴 종료
        if (isCollected)
        {
            yield break;
        }

        yield return new WaitForSeconds(coolTime);      // 쿨타임 대기
        isCoolTime = false;                             // 쿨타임 해제
        selfColider.enabled = true;                     // 콜라이더 다시 활성화
    }    
}


