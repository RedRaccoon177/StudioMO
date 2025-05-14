using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class MainManager : Manager
{
    [Header(nameof(MainManager))]
    [Header("오프닝")]
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
    private static readonly float ObjectHideZoneY = -7.025f;
    private static readonly Vector3 StartPosition = new Vector3(0, 1.36145f, 0);
    private static readonly Vector3 EndPosition = new Vector3(0, -9.83855f, 0);

    [Header("초기 진입창")]
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

    [Header("이전 버튼"), SerializeField]
    private Button backButton;

    [Header("스테이지 모드")]
    [SerializeField]
    private Button[] stageThemeButtons = new Button[StageSelectLength];
    [SerializeField]
    private Sprite[] stageThemeSprites = new Sprite[0];
    [SerializeField]
    private Button stageLeftArrowButton;
    [SerializeField]
    private Button stageRightArrowButton;
    private byte stageCurrentFloor = 0;
    private static readonly int StageSelectLength = 4;
    private static readonly int StageMaxCount = 10;
    private static readonly string StageTag = "Stage";

#if UNITY_EDITOR
    protected override void OnValidate()
    {
        base.OnValidate();
        if(this == instance)
        {
            ExtensionMethod.Sort(ref stageThemeButtons, StageSelectLength);
            ExtensionMethod.Sort(ref stageThemeSprites);
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

    protected override void ChangeText(Translation.Language language)
    {
        base.ChangeText(language);
        stageButton.SetText(Translation.Get(Translation.Letter.Stage));
        pvpButton.SetText(Translation.Get(Translation.Letter.PVP));
        storeButton.SetText(Translation.Get(Translation.Letter.Store));
        optionButton.SetText(Translation.Get(Translation.Letter.Option));
        exitButton.SetText(Translation.Get(Translation.Letter.Exit));
    }

    protected override void Initialize()
    {
        backButton.SetListener(ShowEntry);
        stageButton.SetListener(() =>
        {
            backButton.SetActive(true); //이걸 SetActiveEntryButtons가 false가 될 때 한번에 바꿀 것인지 좀 더 생각할 필요가 있다.
            SetActiveEntryButtons(false);
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
            //여기서 상세 옵션창과 일반 옵션창 분기가 있어서 
        });
        exitButton.SetListener(() =>
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        });
        for(byte i = 0; i < stageThemeButtons.Length; i++)
        {
            byte index = i;
            stageThemeButtons[i].SetListener(() =>
            {
                SetActiveStageThemeButtons(false);
                SelectStageTheme((byte)(index + stageCurrentFloor));
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
            if (stageCurrentFloor < stageThemeSprites.Length - StageSelectLength)
            {
                stageCurrentFloor++;
                SetStageThemeButtonsImage();
            }
        });
        ShowEntry();
        fixedPosition = StartPosition;
        descriptionText.DOFade(1f, openingTime);
        DOVirtual.DelayedCall(openingTime, () => { startKeyDown = true; });
        primaryAction += () =>
        {
            Debug.Log("Primary Action");
        };
        secondaryAction += () =>
        {
            Debug.Log("Secondary Action");
        };
    }

    //초기화면 진입
    private void ShowEntry()
    {
        SetActiveStageThemeButtons(false);
        SetActiveEntryButtons(true);
        backButton.SetActive(false);
    }

    //초기 진입창 버튼들 활성화 여부
    private void SetActiveEntryButtons(bool value)
    {
        stageButton.SetActive(value);
        pvpButton.SetActive(value);
        storeButton.SetActive(value);
        optionButton.SetActive(value);
        exitButton.SetActive(value);
    }

    //스테이지 테마 버튼들 활성화 여부
    private void SetActiveStageThemeButtons(bool value)
    {
        stageThemeButtons.SetActive(value);
        stageLeftArrowButton.SetActive(value);
        stageRightArrowButton.SetActive(value);
    }

    //스테이지 테마 버튼 이미지 설정
    private void SetStageThemeButtonsImage()
    {
        int stageClearCount = PlayerPrefs.GetInt(StageTag, 0);
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

    //특정 스테이지 테마 선택
    private void SelectStageTheme(byte value)
    {
        Debug.Log(value);
        int stageClearCount = PlayerPrefs.GetInt(StageTag, 0);
    }
}