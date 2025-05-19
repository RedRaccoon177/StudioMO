using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectionUICtrl : MonoBehaviour
{
    // �� �����̴� ���δ� ĵ���� ( Ȱ�� / ��Ȱ��ȭ �� )
    [SerializeField] private Canvas collectionSliderCanvas;
    // �� ���� ����� ǥ�����ִ� �����̴�
    [SerializeField] private Slider collectionSlider;

    // �� ä���� �����̴� UI�� ȭ�鿡 ���̰� ��
    public void ShowCollectionSlider() => collectionSliderCanvas.enabled = true;

    // �� ä���� �����̴� UI ����
    public void HideCollectionSlider() => collectionSliderCanvas.enabled = false;

    // �� ä���� �����̴� ( 0 ~ 1 ) ���� ������ �� ����
    public void SliderValueUpdate(float ratio) => collectionSlider.value = ratio; 
}
