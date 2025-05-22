using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
[RequireComponent(typeof(Collider))]
public class Mineral : MonoBehaviour
{
    [Header("광물 최대한도")]
    [SerializeField] uint maxValue;
    private uint currentValue = 0;                              //현재 잔여량
    [Header("1회 채취시 획득량")]
    [SerializeField] uint acquisitionAmount;
    [Header("광물 소진 시 충전시간")]
    [SerializeField] float chargingTime;
    private float remainingTime = 0;                            //남은 충전 시간

    [Header("현재 채취 진행도를 보여주는 슬라이더"),SerializeField]
    private Slider progressSlider;
    private float progressValue = 0;                            //현재 채광 진행도

    private static readonly float ProgressMissValue = 25f;      //채광 빗나감 값
    private static readonly float ProgressCriticalValue = 50f;  //채광 적중 값
    private static readonly float ProgressCompleteValue = 100f; //채광 완료 값

    private void Awake()
    {
        currentValue = maxValue;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            Debug.Log("입력");
            GetMineral(true);
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            Debug.Log("입력");
            GetMineral(false);
        }
        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
            if (remainingTime <= 0)
            {
                //coll.enabled = true;
                remainingTime = 0;
                currentValue = maxValue;
            }
        }
    }

    // ▼ 슬라이더 게이지 증가시키는 함수 + 게이지 잠깐 보였다가 사라짐
    private void UpdateSliderValue(float value)
    {
        if (progressSlider != null)
        {
            progressSlider.value = value;
        }
    }

    // ▼ 채취를 시도했을 때 미네랄 획득량을 반환하는 함수
    public uint GetMineral(bool perfectHit)
    {
        uint value = 0;
        if (currentValue > 0)
        {
            float increaseValue = perfectHit ? ProgressCriticalValue : ProgressMissValue;
            progressValue += increaseValue;
            if (progressValue >= ProgressCompleteValue)
            {
                progressValue = 0;
                currentValue -= 1;
                if(currentValue == 0)
                {
                    remainingTime = chargingTime;
                }
                value = acquisitionAmount;
            }
            UpdateSliderValue(progressValue / ProgressCompleteValue);
        }
        return value;
    }
}
