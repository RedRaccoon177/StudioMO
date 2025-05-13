using UnityEngine;
using TMPro;  // TextMeshPro 관련 네임스페이스 추가
using System.Collections;

public class BlinkingTextTMP : MonoBehaviour
{
    [Header("텍스트 설정")]
    public TextMeshProUGUI blinkingText;  // TextMeshPro용
    public float blinkInterval = 0.5f;

    [Header("등장 설정")]
    public float delayBeforeShow = 3f;

    void Start()
    {
        blinkingText.gameObject.SetActive(false);
        StartCoroutine(ShowAndBlink());
    }

    IEnumerator ShowAndBlink()
    {
        yield return new WaitForSeconds(delayBeforeShow);

        blinkingText.gameObject.SetActive(true);

        while (true)
        {
            blinkingText.enabled = !blinkingText.enabled;
            yield return new WaitForSeconds(blinkInterval);
        }
    }
}
