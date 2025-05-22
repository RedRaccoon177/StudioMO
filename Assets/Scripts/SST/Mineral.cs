using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
[RequireComponent(typeof(Collider))]
public class Mineral : MonoBehaviour
{
    [Header("���� �ִ��ѵ�")]
    [SerializeField] uint maxValue;
    private uint currentValue = 0;                              //���� �ܿ���
    [Header("1ȸ ä��� ȹ�淮")]
    [SerializeField] uint acquisitionAmount;
    [Header("���� ���� �� �����ð�")]
    [SerializeField] float chargingTime;
    private float remainingTime = 0;                            //���� ���� �ð�

    [Header("���� ä�� ���൵�� �����ִ� �����̴�"),SerializeField]
    private Slider progressSlider;
    private float progressValue = 0;                            //���� ä�� ���൵

    private static readonly float ProgressMissValue = 25f;      //ä�� ������ ��
    private static readonly float ProgressCriticalValue = 50f;  //ä�� ���� ��
    private static readonly float ProgressCompleteValue = 100f; //ä�� �Ϸ� ��

    private void Awake()
    {
        currentValue = maxValue;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            Debug.Log("�Է�");
            GetMineral(true);
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            Debug.Log("�Է�");
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

    // �� �����̴� ������ ������Ű�� �Լ� + ������ ��� �����ٰ� �����
    private void UpdateSliderValue(float value)
    {
        if (progressSlider != null)
        {
            progressSlider.value = value;
        }
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
                    remainingTime = chargingTime;
                }
                value = acquisitionAmount;
            }
            UpdateSliderValue(progressValue / ProgressCompleteValue);
        }
        return value;
    }
}
