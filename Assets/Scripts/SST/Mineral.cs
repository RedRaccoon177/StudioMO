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
    [Header("���� �ִ��ѵ�")]
    [SerializeField] uint maxValue;
    private uint currentValue = 0;                              //���� �ܿ���
    [Header("1ȸ ä��� ȹ�淮")]
    [SerializeField] uint acquisitionAmount;
    [Header("���� ���� �� �����ð�")]
    [SerializeField] float chargingTime;
    private float remainingTime = 0;                            //���� ���� �ð�

    [Header("ĵ���� ������Ʈ"), SerializeField]
    private GameObject canvasObject;
    [Header("���� ä�� ���൵�� �����ִ� �����̴�"),SerializeField]
    private Image progressImage;
    private float progressValue = 0;                            //���� ä�� ���൵
    [SerializeField, Range(0, int.MaxValue)]
    private float progressFadeTime = 0.8f;

    public bool collectable {
        get
        {
            return currentValue > 0;
        }
    }

    private static readonly float ProgressMissValue = 25f;      //ä�� ������ ��
    private static readonly float ProgressCriticalValue = 50f;  //ä�� ���� ��
    private static readonly float ProgressCompleteValue = 100f; //ä�� �Ϸ� ��

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

    // �� �̹��� ������ ������Ű�� �Լ� + ������ ��� �����ٰ� �����
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

    //ĵ������ ��� ������ ������� �� �ڷ�ƾ �Լ�
    private IEnumerator ShowCanvas()
    {
        SetActiveCanvas(true);
        yield return new WaitForSeconds(progressFadeTime);
        SetActiveCanvas(false);
    }

    // �� ä�븦 �õ����� �� �̳׶� ȹ�淮�� ��ȯ�ϴ� �Լ�
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