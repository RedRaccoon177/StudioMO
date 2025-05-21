using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using TMPro;

public static class ExtensionMethod
{
    public static void Set(this Transform transform, Vector3 position, Quaternion rotation)
    {
        if (transform != null)
        {
            transform.position = position;
            transform.rotation = rotation;
        }
    }

    public static void Set(this Image image, Sprite sprite)
    {
        if(image != null)
        {
            image.sprite = sprite;
        }
    }

    public static void Fill(this Image image, float value)
    {
        if(image != null)
        {
            image.fillAmount = value;
        }
    }

    public static void SetActive(this TMP_Text tmpText, bool value)
    {
        if (tmpText != null)
        {
            tmpText.gameObject.SetActive(value);
        }
    }

    public static void Set(this TMP_Text tmpText, string value)
    {
        if (tmpText != null)
        {
            tmpText.text = value;
        }
    }

    public static void Set(this TMP_Text tmpText, string value, TMP_FontAsset tmpFontAsset)
    {
        if (tmpText != null)
        {
            tmpText.font = tmpFontAsset;
            tmpText.text = value;
        }
    }

    public static void Set(this TMP_Text tmpText, string value, bool active)
    {
        if(tmpText != null)
        {
            tmpText.text = value;
            tmpText.gameObject.SetActive(active);
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

    public static void SetText(this Button button, string value, TMP_FontAsset tmpFontAsset)
    {
        if (button != null)
        {
            TextMeshProUGUI[] tmpTexts = button.GetComponentsInChildren<TextMeshProUGUI>();
            foreach (TextMeshProUGUI tmpText in tmpTexts)
            {
                tmpText.font = tmpFontAsset;
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

    public static void Sort<T>(ref T[] array) where T : UnityEngine.Object
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

    public static void Sort<T>(ref T[] array, int length) where T : UnityEngine.Object
    {
        Sort(ref array);
        T[] templates = new T[length];
        for (int i = 0; i < Mathf.Clamp(array.Length, 0, length); i++)
        {
            templates[i] = array[i];
        }
        array = templates;
    }

    public static void Set(this InputActionReference inputActionReference, bool value, Action<InputAction.CallbackContext> action)
    {
        if (inputActionReference != null && inputActionReference.action != null)
        {
            if (value == true)
            {
                inputActionReference.action.performed += action;
                inputActionReference.action.Enable();
            }
            else
            {
                inputActionReference.action.performed -= action;
                inputActionReference.action.Disable();
            }
        }
    }
}