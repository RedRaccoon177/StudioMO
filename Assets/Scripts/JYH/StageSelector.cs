using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[DisallowMultipleComponent]
[RequireComponent(typeof(Button))]
public class StageSelector : MonoBehaviour
{
    private bool hasButton = false;

    private Button button = null;

    private Button getButton
    {
        get
        {
            if (hasButton == false)
            {
                button = GetComponent<Button>();
                hasButton = true;
            }
            return button;
        }
    }

    [SerializeField]
    private TMP_Text numberText;

    [SerializeField]
    private Sprite enableStarSprite;
    [SerializeField]
    private Sprite disableStarSprite;

    private static readonly int StarCount = 2;
    [SerializeField]
    private Image[] starImages = new Image[StarCount];

    private bool clear = false;

    private StageData stageData = null;

#if UNITY_EDITOR

    private void OnValidate()
    {
        Transform parent = transform.parent;
        if(parent != null)
        {
            for (int i = 0; i < parent.childCount; i++)
            {
                if (parent.GetChild(i) == transform)
                {
                    numberText.Set((i + 1).ToString());
                    break;
                }
            }
        }
        ExtensionMethod.Sort(ref starImages, StarCount);
    }
#endif

    public void Initialize(Action<StageData, bool> action)
    {
        getButton.onClick.RemoveAllListeners();
        getButton.onClick.AddListener(() =>
        {
            action?.Invoke(stageData, clear);
        });
    }

    public void Show(bool? clear, StageData stageData)
    {
        gameObject.SetActive(true);
        if (clear == null)
        {
            this.clear = false;
            getButton.interactable = false;
            for (int i = 0; i < starImages.Length; i++)
            {
                starImages[i].Set(disableStarSprite);
            }
        }
        else
        {
            this.clear = clear.Value;
            for (int i = 0; i < starImages.Length; i++)
            {
                if (i == 0 || this.clear == true)
                {
                    starImages[i].Set(enableStarSprite);
                }
                else
                {
                    starImages[i].Set(disableStarSprite);
                }
            }
            getButton.interactable = true;
        }
        this.stageData = stageData;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}