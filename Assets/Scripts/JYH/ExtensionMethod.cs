using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public static class ExtensionMethod
{
    public static void SetActive(this TMP_Text tmpText, bool value)
    {
        if (tmpText != null)
        {
            tmpText.gameObject.SetActive(value);
        }
    }

    public static void SetActive(this TMP_Text tmpText, bool value, float alpha)
    {
        if (tmpText != null)
        {
            Color color = tmpText.color;
            color.a = alpha;
            tmpText.color = color;
            tmpText.gameObject.SetActive(value);
        }
    }


    public static void SetListener(this Button button, UnityAction action)
    {
        if (button != null)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(action);
        }
    }

    public static void SetText(this Button button, string value)
    {
        if (button != null)
        {
            TextMeshProUGUI[] tmpTexts = button.GetComponentsInChildren<TextMeshProUGUI>();
            foreach (TextMeshProUGUI tmpText in tmpTexts)
            {
                tmpText.text = value;
            }
        }
    }
}