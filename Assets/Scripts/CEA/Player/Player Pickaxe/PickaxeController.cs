using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class PickaxeController : MonoBehaviour
{
    private PlayerController player;

    private float normalSuccessGage = 10; //일반 성공 시 게이지 상승량
    private float perfectSuccessGage = 25; //대성공 시 게이지 상승량

    private bool hitA = false;
    private bool waitForB = false;
    private bool hitC = false;

    private float timerB = 0f;

    private void Awake()
    {
        player = GetComponentInParent<PlayerController>();
    }

    private void Update()
    {
        if (waitForB)
        {
            timerB -= Time.deltaTime;

            if(timerB <= 0f)
            {
                waitForB = false;

                if(hitA || hitC) //타이머가 지났는데 b에 닿지 않았을 경우
                {
                    Debug.Log("성공");
                }
                ResetFlags();
            }
        }
    }

    public void ReportHit(string hitboxID, CollectionObject item)
    {
        switch(hitboxID)
        {
            case "A": //A에 닿았을 경우

                hitA = true;
                StartBTimer();
                break;

            case "C": //C에 닿았을 경우

                hitC = true;
                StartBTimer();
                break;

            case "B": //B까지 도달했을 경우

                if(waitForB && (hitA || hitC)) //시간이 0.3초를 넘지 않았으며 a 또는 c에 닿았을 경우
                {
                    Debug.Log("대성공");
                    item.TryAddCollectGage(perfectSuccessGage);
                }

                ResetFlags();

                break;
        }
    }

    /// <summary>
    /// 단위 시간동안 B에 닿는지 판별하는 함수
    /// </summary>
    private void StartBTimer()
    {
        waitForB = true;
        timerB = 0.3f;
    }

    /// <summary>
    /// 상호작용 상태 초기화 함수
    /// </summary>
    private void ResetFlags()
    {
        hitA = false;
        hitC = false;
        waitForB = false;
        timerB = 0f;
    }


    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("Interactable") && player.RightSelectOn)
    //    {
    //        Debug.Log("채집");
    //        item = other.gameObject.GetComponent<CollectionObject>();

    //        if(item != null)
    //        {
    //            item._Slider.value += collectGage;

    //            if (item._Slider.value >= 100)
    //            {
    //                Debug.Log("채집 완료");
    //                item.CollectCompleted();    
    //            }
    //        }
    //    }
    //}
}
