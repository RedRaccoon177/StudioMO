using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class MainManager : Manager
{
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
    private static readonly float ObjectHideZoneY = -7.025f;
    private static readonly Vector3 StartPosition = new Vector3(0, 1.36145f, 0);
    private static readonly Vector3 EndPosition = new Vector3(0, -9.83855f, 0);

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

    [Header("���� ��ư"), SerializeField]
    private Button backButton;

    [Header("�������� ���")]
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
            backButton.SetActive(true); //�̰� SetActiveEntryButtons�� false�� �� �� �ѹ��� �ٲ� ������ �� �� ������ �ʿ䰡 �ִ�.
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
            //���⼭ �� �ɼ�â�� �Ϲ� �ɼ�â �бⰡ �־ 
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

    //�ʱ�ȭ�� ����
    private void ShowEntry()
    {
        SetActiveStageThemeButtons(false);
        SetActiveEntryButtons(true);
        backButton.SetActive(false);
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

    //Ư�� �������� �׸� ����
    private void SelectStageTheme(byte value)
    {
        Debug.Log(value);
        int stageClearCount = PlayerPrefs.GetInt(StageTag, 0);
    }
}