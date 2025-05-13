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

    private static readonly Vector3 FixedPosition = new Vector3(0, 1.36144f, 0);

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

    }

    protected override void Initialize()
    {
        fixedPosition = FixedPosition;
        descriptionText.DOFade(1f, openingTime);
        DOVirtual.DelayedCall(openingTime, () =>
        {
            startKeyDown = true;
        });
    }
}
