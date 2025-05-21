using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectionUICtrl : MonoBehaviour
{
    // ▼ 슬라이더 감싸는 캔버스 ( 활성 / 비활성화 용 )
    [SerializeField] private Canvas collectionSliderCanvas;
    // ▼ 실제 진행률 표시해주는 슬라이더
    [SerializeField] private Slider collectionSlider;

    // ▼ 채집물 슬라이더 UI를 화면에 보이게 함
    public void ShowCollectionSlider() => collectionSliderCanvas.enabled = true;

    // ▼ 채집물 슬라이더 UI 숨김
    public void HideCollectionSlider() => collectionSliderCanvas.enabled = false;

    // ▼ 채집물 슬라이더 ( 0 ~ 1 ) 사이 비율로 값 조절
    public void SliderValueUpdate(float ratio) => collectionSlider.value = ratio; 
}
