using UnityEngine;
using TMPro;  // TextMeshPro ���� ���ӽ����̽� �߰�
using System.Collections;

public class BlinkingTextTMP : MonoBehaviour
{
    [Header("�ؽ�Ʈ ����")]
    public TextMeshProUGUI blinkingText;  // TextMeshPro��
    public float blinkInterval = 0.5f;

    [Header("���� ����")]
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
