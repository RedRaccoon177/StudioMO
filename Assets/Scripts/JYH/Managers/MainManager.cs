using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using DG.Tweening;

public class MainManager : Manager
{
    public static readonly string SceneName = "MainScene";

    [Header(nameof(MainManager))]
    [Header("������")]
    [SerializeField]
    private TMP_Text titleText;
    [SerializeField]
    private TMP_Text descriptionText;
    [SerializeField]
    private GameObject canvasBlockObject;
    [SerializeField, Range(0, float.MaxValue)]
    private float openingTime = 3.0f;
    [SerializeField, Range(0, float.MaxValue)]
    private float pivotMoveTime = 1;
    private bool startKeyDown = false;
    private static readonly float ObjectHideZoneY = -12.50436f;
    private static readonly Vector3 StartPosition = new Vector3(0, 1.36144f, 0);
    private static readonly Vector3 EndPosition = new Vector3(0, -17.26856f, 0);

    [Header("�ʱ� ����â")]
    [SerializeField]
    private Button stageButton;
    [SerializeField]
    private Button pvpButton;
    [SerializeField]
    private Button storeButton;
    [SerializeField]
    private Button optionButton;
    [SerializeField]
    private Button exitButton;
    [SerializeField]
    private Image backgroundImage;

    [Header("���� ��ư"), SerializeField]
    private Button backButton;

    [Header("�������� ���")]
    [SerializeField]
    private Button[] stageThemeButtons = new Button[StageThemeLength];
    [SerializeField]
    private Sprite[] stageThemeSprites = new Sprite[0];
    [SerializeField]
    private Button stageLeftArrowButton;
    [SerializeField]
    private Button stageRightArrowButton;
    private byte stageCurrentFloor = 0;
    private static readonly int StageThemeLength = 4;
    private static readonly int StageMaxCount = 10;
    private static readonly string StageTag = "Stage";

    [SerializeField]
    private StageData[] stageDatas = new StageData[0];
    [SerializeField]
    private StageSelector[] stageSelectors = new StageSelector[StageMaxCount];
    [SerializeField]
    private TMP_Text stageStarCountText;
    [SerializeField]
    private StageSelectPanel stageSelectPanel;

    [Header("�ɼ�")]
    [SerializeField]
    private SettingPanel settingPanel;

    [Header("������")]
    [SerializeField]
    private ExitPanel exitPanel;

#if UNITY_EDITOR
    protected override void OnValidate()
    {
        base.OnValidate();
        if(this == instance)
        {
            ExtensionMethod.Sort(ref stageThemeButtons, StageThemeLength);
            ExtensionMethod.Sort(ref stageThemeSprites);
            ExtensionMethod.Sort(ref stageSelectors, StageMaxCount);
        }
    }
#endif

    protected override void Update()
    {
        base.Update();
        if(startKeyDown == true && Input.anyKeyDown)
        {
            startKeyDown = false;
            Vector3 value = StartPosition;
            DOTween.To(() => value, x => value = x, EndPosition, pivotMoveTime).SetEase(Ease.Linear).OnUpdate(() =>
              {
                  fixedPosition = value;
                  if(value.y < ObjectHideZoneY && canvasBlockObject != null && canvasBlockObject.activeSelf == true)
                  {
                      canvasBlockObject.SetActive(false);
                  }
              });
        }
    }

    protected override void Initialize()
    {
        backButton.SetListener(ShowEntry);
        stageButton.SetListener(() =>
        {
            backButton.SetActive(true);
            SetActiveEntryButtons(false);
            SetStageThemeButtonsImage();
            SetActiveStageThemeButtons(true);
        });
        pvpButton.SetListener(() =>
        {
            backButton.SetActive(true);
            SetActiveEntryButtons(false);
        });
        storeButton.SetListener(() =>
        {
            backButton.SetActive(true);
            SetActiveEntryButtons(false);
        });
        optionButton.SetListener(() =>
        {
            settingPanel?.Open();
        });
        exitButton.SetListener(() => { exitPanel?.Open(); });
        for(int i = 0; i < stageThemeButtons.Length; i++)
        {
            int index = i;
            stageThemeButtons[i].SetListener(() =>
            {
                SetActiveStageThemeButtons(false);
                Sprite sprite = stageThemeButtons[index] != null && stageThemeButtons[index].image != null ? stageThemeButtons[index].image.sprite : null;
                backgroundImage.Set(sprite);
                stageSelectPanel?.SetThemeImage(sprite);
                int stageMinIndex = (index + stageCurrentFloor) * StageMaxCount;
                int stageClearCount = PlayerPrefs.GetInt(StageTag);
                int starCount = 0;
                for (int i = 0; i < stageSelectors.Length; i++)
                {
                    StageData stageData = stageDatas.Length > stageMinIndex + i ? stageDatas[stageMinIndex + i]: null;
                    if (stageMinIndex + i < stageClearCount)
                    {
                        starCount += 2;
                        stageSelectors[i]?.Show(true, stageData);
                    }
                    else if (stageMinIndex + i == stageClearCount)
                    {
                        starCount++;
                        stageSelectors[i]?.Show(false, stageData);
                    }
                    else
                    {
                        stageSelectors[i]?.Show(null, stageData);
                    }
                }
                stageStarCountText.Set("= " + starCount.ToString(), true);
            });
        }
        stageLeftArrowButton.SetListener(() =>
        {
            if(stageCurrentFloor > 0)
            {
                stageCurrentFloor--;
                SetStageThemeButtonsImage();
            }
        });
        stageRightArrowButton.SetListener(() =>
        {
            if (stageCurrentFloor < stageThemeSprites.Length - StageThemeLength)
            {
                stageCurrentFloor++;
                SetStageThemeButtonsImage();
            }
        });
        for (int i = 0; i < stageSelectors.Length; i++)
        {
            stageSelectors[i]?.Initialize((stageData, clear) => { stageSelectPanel?.Open(stageData, clear); });
        }
        stageSelectPanel?.Initialize(() => { SceneManager.LoadScene(StageManager.SceneName); });
        settingPanel?.Initialize(SetLanguage);
        ShowEntry();
        fixedPosition = StartPosition;
        descriptionText.DOFade(1f, openingTime);
        DOVirtual.DelayedCall(openingTime, () => { startKeyDown = true; });
        primaryAction += () => ShowEntry();
    }

    protected override void ChangeText()
    {
        stageButton.SetText(Translation.Get(Translation.Letter.Stage), tmpFontAsset);
        pvpButton.SetText(Translation.Get(Translation.Letter.PVP), tmpFontAsset);
        storeButton.SetText(Translation.Get(Translation.Letter.Store), tmpFontAsset);
        optionButton.SetText(Translation.Get(Translation.Letter.Option), tmpFontAsset);
        exitButton.SetText(Translation.Get(Translation.Letter.ExitGame), tmpFontAsset);
        stageSelectPanel?.ChangeText();
        exitPanel?.ChangeText();
    }

    //�ʱ�ȭ�� ����
    private void ShowEntry()
    {
        backButton.SetActive(false);
        SetActiveStageThemeButtons(false);
        stageStarCountText.SetActive(false);
        for (int i = 0; i < stageSelectors.Length; i++)
        {
            stageSelectors[i]?.Hide();
        }
        stageSelectPanel?.Close();
        backgroundImage.Set(null);
        SetActiveEntryButtons(true);
    }

    //�ʱ� ����â ��ư�� Ȱ��ȭ ����
    private void SetActiveEntryButtons(bool value)
    {
        stageButton.SetActive(value);
        pvpButton.SetActive(value);
        storeButton.SetActive(value);
        optionButton.SetActive(value);
        exitButton.SetActive(value);
    }

    //�������� �׸� ��ư�� Ȱ��ȭ ����
    private void SetActiveStageThemeButtons(bool value)
    {
        stageThemeButtons.SetActive(value);
        stageLeftArrowButton.SetActive(value);
        stageRightArrowButton.SetActive(value);
    }

    //�������� �׸� ��ư �̹��� ����
    private void SetStageThemeButtonsImage()
    {
        int stageClearCount = PlayerPrefs.GetInt(StageTag);
        for (int i = 0; i < stageThemeButtons.Length; i++)
        {
            if (stageThemeSprites.Length > i + stageCurrentFloor)
            {
                stageThemeButtons[i].SetImage(stageThemeSprites[i + stageCurrentFloor], (i + stageCurrentFloor) * StageMaxCount <= stageClearCount);
            }
            else
            {
                stageThemeButtons[i].SetImage(null, false);
            }
        }
    }
}