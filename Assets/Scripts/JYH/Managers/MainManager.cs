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
    private Transform coverPanel;
    [SerializeField, Range(0, float.MaxValue)]
    private float openingTime = 3.0f;
    [SerializeField, Range(0, float.MaxValue)]
    private float coverMoveTime = 0.5f;
    private bool startKeyDown = false;
    private static readonly Vector3 MoveCoverPosition = new Vector3(0.00f, 7.03f, 4.80f);
    [Header("기본 선택창")]
    [SerializeField]
    private Button stageModeButton;
    [SerializeField]
    private Button matchModeButton;
    [SerializeField]
    private Button shopButton;
    [SerializeField]
    private Button optionButton;
    [SerializeField]
    private Button exitButton;
    [SerializeField]
    private Button[] themeSelectButtons = new Button[themeSelectButtonsLength];
    private static readonly int themeSelectButtonsLength = 4;

    private static readonly Vector3 FixedPosition = new Vector3(0, 1.36144f, 0);

#if UNITY_EDITOR
    protected override void OnValidate()
    {
        base.OnValidate();
        if(this == instance && themeSelectButtons.Length != themeSelectButtonsLength)
        {
            Button[] buttons = new Button[themeSelectButtonsLength];
            for (int i = 0; i < Mathf.Clamp(themeSelectButtons.Length, 0, themeSelectButtonsLength); i++)
            {
                buttons[i] = themeSelectButtons[i];
            }
            themeSelectButtons = buttons;
        }
    }
#endif

    protected override void Update()
    {
        base.Update();
        if(startKeyDown == true && Input.anyKeyDown)
        {
            startKeyDown = false;
            titleText.SetActive(false);
            descriptionText.SetActive(false);
            coverPanel.DOMove(MoveCoverPosition, coverMoveTime).SetEase(Ease.InOutSine);
        }
    }

    protected override void ChangeText(Translation.Language language)
    {
        base.ChangeText(language);
        stageModeButton.SetText(Translation.Get(Translation.Letter.Stage) + " " + Translation.Get(Translation.Letter.Mode));
        matchModeButton.SetText(Translation.Get(Translation.Letter.Match) + " " + Translation.Get(Translation.Letter.Mode));
        shopButton.SetText(Translation.Get(Translation.Letter.Shop));
        optionButton.SetText(Translation.Get(Translation.Letter.Option));
        exitButton.SetText(Translation.Get(Translation.Letter.Exit));
    }

    protected override void Initialize()
    {
        stageModeButton.SetListener(() =>
        {
            SetActiveEntryButtons(false);

        });

        exitButton.SetListener(() =>
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        });
        fixedPosition = FixedPosition;
        descriptionText.DOFade(1f, openingTime);
        DOVirtual.DelayedCall(openingTime, () =>
        {
            startKeyDown = true;
        });
    }

    private void SetActiveEntryButtons(bool value)
    {
        stageModeButton.SetActive(value);
        matchModeButton.SetActive(value);
        shopButton.SetActive(value);
        optionButton.SetActive(value);
        exitButton.SetActive(value);
    }
}
