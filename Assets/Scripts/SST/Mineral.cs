using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

[DisallowMultipleComponent]
[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PhotonView))]
public class Mineral : MonoBehaviourPunCallbacks
{
    [Header("광물 최대한도")]
    [SerializeField] uint maxValue;
    private uint currentValue = 0;                              //현재 잔여량
    [Header("1회 채취시 획득량")]
    [SerializeField] uint acquisitionAmount;
    [Header("광물 소진 시 충전시간")]
    [SerializeField] float chargingTime;
    private float remainingTime = 0;                            //남은 충전 시간

    [Header("캔버스 오브젝트"), SerializeField]
    private GameObject canvasObject;
    [Header("현재 채취 진행도를 보여주는 슬라이더"),SerializeField]
    private Image progressImage;
    private float progressValue = 0;                            //현재 채광 진행도
    [SerializeField, Range(0, int.MaxValue)]
    private float progressFadeTime = 0.8f;

    public bool collectable {
        get
        {
            return currentValue > 0;
        }
    }

    private static readonly float ProgressMissValue = 25f;      //채광 빗나감 값
    private static readonly float ProgressCriticalValue = 50f;  //채광 적중 값
    private static readonly float ProgressCompleteValue = 100f; //채광 완료 값

    private void Awake()
    {
        currentValue = maxValue;
    }

    private void Update()
    {
        if (photonView.IsMine == true && remainingTime > 0)
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

    public override void OnEnable()
    {
        base.OnEnable();
        SetActiveCanvas(false);
    }

    public override void OnDisable()
    {
        base.OnDisable();
        StopAllCoroutines();
    }

    // ▼ 이미지 게이지 증가시키는 함수 + 게이지 잠깐 보였다가 사라짐
    private void ShowImageValue(float value)
    {
        if (progressImage != null)
        {
            progressImage.fillAmount = value;
        }
        StopAllCoroutines();
        StartCoroutine(ShowCanvas());
    }

    private void SetActiveCanvas(bool value)
    {
        if (canvasObject != null)
        {
            canvasObject.SetActive(value);
        }
    }

    //캔버스를 잠시 보였다 사라지게 할 코루틴 함수
    private IEnumerator ShowCanvas()
    {
        SetActiveCanvas(true);
        yield return new WaitForSeconds(progressFadeTime);
        SetActiveCanvas(false);
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
                    if (chargingTime > 0)
                    {
                        remainingTime = chargingTime;
                    }
                    else
                    {
                        currentValue = maxValue;
                    }
                }
                value = acquisitionAmount;
            }
            ShowImageValue(progressValue / ProgressCompleteValue);
        }
        return value;
    }
}