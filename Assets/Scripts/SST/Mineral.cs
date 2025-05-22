using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Mineral : MonoBehaviour
{
    [Header("광물 최대한도")]
    [SerializeField] uint maxValue;

    private uint currentValue = 0;

    [Header("1회 채취시 획득량")]
    [SerializeField] uint getAmount;

    [Header("광물 소진 시 충전시간")]
    [SerializeField] float chargeTime;

    private float currentChargeTime = 0;

    [Header("광물 게이지 UI")]
    [SerializeField] private Canvas collectionSliderCanvas; // 슬라이더 감싸는 캔버스 ( 활성 / 비활성화 용 )
    [SerializeField] private Slider collectionSlider; // 실제 진행률 표시해주는 슬라이더

    MeshRenderer meshRender;
    Collider coll;

    // ▼ 채집물 슬라이더 UI를 화면에 보이게 함
    public void ShowCollectionSlider() => collectionSliderCanvas.enabled = true;

    // ▼ 채집물 슬라이더 UI 숨김
    public void HideCollectionSlider() => collectionSliderCanvas.enabled = false;

    // ▼ 채집물 슬라이더 0 ~ 100 값 조절
    public void SliderValueUpdate(float value) => collectionSlider.value = value;

    private void Awake()
    {
        currentValue = maxValue;
        collectionSlider.minValue = 0f;
        collectionSlider.maxValue = 100f;

        meshRender = GetComponent<MeshRenderer>();
        coll = GetComponent<Collider>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GetMineral(true);
        }

        //광물이 모두 소진 시 충전시간이 동작하는데 이 충전시간이 제시한 chargeTime에 도달하면 
        //광물의 현재 수량을 광물최대한도 값으로 대입한다.
        if (currentValue == 0 && currentChargeTime < chargeTime)
        {
            meshRender.enabled = false;
            coll.enabled = false;
            currentChargeTime += Time.deltaTime;

            if (currentChargeTime >= chargeTime)
            {
                meshRender.enabled = true;
                coll.enabled = true;
                currentValue = maxValue;
                currentChargeTime = 0;
            }
        }        
    }

    // ▼ 광물 채집 한도 수 감소시키는 함수
    public void DecreaseCurrentValue()
    {
        currentValue -= 1;
        Debug.Log("현재 광물 한도 수가 1 줄었습니다.");
        Debug.Log($"현재 광물 최대 채집 제한수는 {currentValue} 입니다.");
    }

    // ▼ 슬라이더 게이지 증가시키는 함수 + 게이지 잠깐 보였다가 사라짐
    public uint UpdateMineralSliderValue(float value)
    {
        float newValue = collectionSlider.value + value;

        if (newValue >= collectionSlider.maxValue)
        {
            collectionSlider.value = collectionSlider.minValue;
            DecreaseCurrentValue();
            Debug.Log("채집 슬라이더 게이지가 최대치에 도달. 채집 성공!");
            Debug.Log($"{getAmount} 의 자원량을 플레이어게게 넘겨줍니다.");
            return getAmount;
        }

        collectionSliderCanvas.enabled = true;
        collectionSlider.value = newValue;

        Debug.Log($"슬라이더 게이지가 {value}만큼 증가했습니다. 현재값: {collectionSlider.value}");

        return 0;
    }

    public uint GetMineral(bool perfectHit)
    {
        float increaseValue = perfectHit ? 50f : 25f;

        return UpdateMineralSliderValue(increaseValue);
    }

    //IEnumerator SliderCanvasRoutine()
    //{
    //    yield return new WaitForSeconds(2f);

    //    collectionSliderCanvas.enabled = false;
    //}
}
