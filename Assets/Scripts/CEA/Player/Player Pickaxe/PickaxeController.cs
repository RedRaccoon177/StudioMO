using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class PickaxeController : MonoBehaviour
{
    private PlayerController player;

    private float normalSuccessGage = 10; //�Ϲ� ���� �� ������ ��·�
    private float perfectSuccessGage = 25; //�뼺�� �� ������ ��·�

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

                if(hitA || hitC) //Ÿ�̸Ӱ� �����µ� b�� ���� �ʾ��� ���
                {
                    Debug.Log("����");
                }
                ResetFlags();
            }
        }
    }

    public void ReportHit(string hitboxID, CollectionObject item)
    {
        switch(hitboxID)
        {
            case "A": //A�� ����� ���

                hitA = true;
                StartBTimer();
                break;

            case "C": //C�� ����� ���

                hitC = true;
                StartBTimer();
                break;

            case "B": //B���� �������� ���

                if(waitForB && (hitA || hitC)) //�ð��� 0.3�ʸ� ���� �ʾ����� a �Ǵ� c�� ����� ���
                {
                    Debug.Log("�뼺��");
                    item.TryAddCollectGage(perfectSuccessGage);
                }

                ResetFlags();

                break;
        }
    }

    /// <summary>
    /// ���� �ð����� B�� ����� �Ǻ��ϴ� �Լ�
    /// </summary>
    private void StartBTimer()
    {
        waitForB = true;
        timerB = 0.3f;
    }

    /// <summary>
    /// ��ȣ�ۿ� ���� �ʱ�ȭ �Լ�
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
    //        Debug.Log("ä��");
    //        item = other.gameObject.GetComponent<CollectionObject>();

    //        if(item != null)
    //        {
    //            item._Slider.value += collectGage;

    //            if (item._Slider.value >= 100)
    //            {
    //                Debug.Log("ä�� �Ϸ�");
    //                item.CollectCompleted();    
    //            }
    //        }
    //    }
    //}
}
