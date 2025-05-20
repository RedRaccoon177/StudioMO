using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class PickaxeController : MonoBehaviour
{
    private PlayerController player;

    public GameObject[] hitboxs;

    private float normalSuccessGage = 10;
    private float perfectSuccessGage = 25;

    private bool hitA = false;
    private bool waitForB = false;
    private bool hitC = false;

    private float timerB = 0f;

    private void Update()
    {
        if (waitForB)
        {
            timerB -= Time.deltaTime;

            if(timerB <= 0f)
            {
                waitForB = false;

                if(hitA || hitC)
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
            case "A":

                hitA = true;
                StartBTimer();
                break;

            case "C":

                hitC = true;
                StartBTimer();
                break;

            case "B":

                if(waitForB && (hitA || hitC))
                {
                    Debug.Log("대성공");
                }

                ResetFlags();

                break;
        }
    }

    private void StartBTimer()
    {
        waitForB = true;
        timerB = 0.3f;
    }

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
