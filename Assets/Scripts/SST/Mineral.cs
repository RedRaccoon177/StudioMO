using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Mineral : MonoBehaviour
{
    [Header("���� �ִ��ѵ�")]
    [SerializeField] uint maxValue;

    private uint currentValue = 0;

    [Header("1ȸ ä��� ȹ�淮")]
    [SerializeField] uint getAmount;

    [Header("���� ���� �� �����ð�")]
    [SerializeField] float chargeTime;

    private float currentChargeTime = 0;

    [Header("���� ������ UI")]
    [SerializeField] private Canvas collectionSliderCanvas; // �����̴� ���δ� ĵ���� ( Ȱ�� / ��Ȱ��ȭ �� )
    [SerializeField] private Slider collectionSlider; // ���� ����� ǥ�����ִ� �����̴�

    MeshRenderer meshRender;
    Collider coll;

    // �� ä���� �����̴� UI�� ȭ�鿡 ���̰� ��
    public void ShowCollectionSlider() => collectionSliderCanvas.enabled = true;

    // �� ä���� �����̴� UI ����
    public void HideCollectionSlider() => collectionSliderCanvas.enabled = false;

    // �� ä���� �����̴� 0 ~ 100 �� ����
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

        //������ ��� ���� �� �����ð��� �����ϴµ� �� �����ð��� ������ chargeTime�� �����ϸ� 
        //������ ���� ������ �����ִ��ѵ� ������ �����Ѵ�.
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

    // �� ���� ä�� �ѵ� �� ���ҽ�Ű�� �Լ�
    public void DecreaseCurrentValue()
    {
        currentValue -= 1;
        Debug.Log("���� ���� �ѵ� ���� 1 �پ����ϴ�.");
        Debug.Log($"���� ���� �ִ� ä�� ���Ѽ��� {currentValue} �Դϴ�.");
    }

    // �� �����̴� ������ ������Ű�� �Լ� + ������ ��� �����ٰ� �����
    public uint UpdateMineralSliderValue(float value)
    {
        float newValue = collectionSlider.value + value;

        if (newValue >= collectionSlider.maxValue)
        {
            collectionSlider.value = collectionSlider.minValue;
            DecreaseCurrentValue();
            Debug.Log("ä�� �����̴� �������� �ִ�ġ�� ����. ä�� ����!");
            Debug.Log($"{getAmount} �� �ڿ����� �÷��̾�԰� �Ѱ��ݴϴ�.");
            return getAmount;
        }

        collectionSliderCanvas.enabled = true;
        collectionSlider.value = newValue;

        Debug.Log($"�����̴� �������� {value}��ŭ �����߽��ϴ�. ���簪: {collectionSlider.value}");

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
