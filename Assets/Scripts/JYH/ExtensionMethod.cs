using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using System.Collections.Generic;

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

    public static void SetImage(this Button button, Sprite sprite)
    {
        if (button != null && button.image != null)
        {
            button.image.sprite = sprite;
        }
    }

    public static void SetImage(this Button button, Sprite sprite, bool interactable)
    {
        if (button != null)
        {
            if (button.image != null)
            {
                button.image.sprite = sprite;
            }
            button.interactable = interactable;
        }
    }

    public static void SetActive(this Button button, bool value)
    {
        if (button != null)
        {
            button.gameObject.SetActive(value);
        }
    }

    public static void SetActive(this Button[] buttons, bool value)
    {
        int length = buttons != null ? buttons.Length: 0;
        for (int i = 0; i < length; i++)
        {
            buttons[i].SetActive(value);
        }
    }

    public static void Sort<T>(ref T[] array) where T : Object
    {
        List<T> list = new List<T>();
        int empty = 0;
        int length = array != null ? array.Length : 0;
        for (int i = 0; i < length; i++)
        {
            T value = array[i];
            if (value != null)
            {
                if (list.Contains(value) == false)
                {
                    list.Add(value);
                }
                else
                {
                    empty++;
                }
            }
            else
            {
                empty++;
            }
        }
        for (int i = 0; i < empty; i++)
        {
            list.Add(null);
        }
        array = list.ToArray();
    }

    public static void Sort<T>(ref T[] array, int length) where T : Object
    {
        Sort(ref array);
        T[] templates = new T[length];
        for (int i = 0; i < Mathf.Clamp(array.Length, 0, length); i++)
        {
            templates[i] = array[i];
        }
        array = templates;
    }
}