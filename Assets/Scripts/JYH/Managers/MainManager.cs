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

    [Header("기본 선택창")]
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

    [Header("스테이지 모드")]
    [SerializeField]
    private Button[] stageSelectButtons = new Button[StageSelectButtonsLength];
    private static readonly int StageSelectButtonsLength = 4;
    [SerializeField]
    private Button stageLeftArrowButton;
    [SerializeField]
    private Button stageRightArrowButton;

#if UNITY_EDITOR
    protected override void OnValidate()
    {
        base.OnValidate();
        if(this == instance)
        {
            ExtensionMethod.Sort(ref stageSelectButtons, StageSelectButtonsLength);
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
        stageButton.SetListener(() =>
        {
            SetActiveEntryButtons(false);
            stageSelectButtons.SetActive(true);
            stageLeftArrowButton.SetActive(true);
            stageRightArrowButton.SetActive(true);
        });
        pvpButton.SetListener(() =>
        {
            SetActiveEntryButtons(false);
        });
        storeButton.SetListener(() =>
        {
            SetActiveEntryButtons(false);
        });
        optionButton.SetListener(() =>
        {

        });
        exitButton.SetListener(() =>
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        });
        fixedPosition = StartPosition;
        descriptionText.DOFade(1f, openingTime);
        DOVirtual.DelayedCall(openingTime, () => { startKeyDown = true; });
    }

    private void SetActiveEntryButtons(bool value)
    {
        stageButton.SetActive(value);
        pvpButton.SetActive(value);
        storeButton.SetActive(value);
        optionButton.SetActive(value);
        exitButton.SetActive(value);
    }
}
