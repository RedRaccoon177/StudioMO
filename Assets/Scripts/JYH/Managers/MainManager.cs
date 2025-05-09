using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class MainManager : Manager
{
    [Header(nameof(MainManager))]
    [SerializeField]
    private TMP_Text titleText;
    [SerializeField]
    private TMP_Text descriptionText;
    [SerializeField]
    private Button backgroundButton;
    [SerializeField, Range(0, float.MaxValue)]
    private float openingTime = 3.0f;
    

    private static readonly Vector3 StartPosition = new Vector3(0, 1.36144f, 1);

    protected override void Update()
    {
        base.Update();
    }

    protected override void Initialize()
    {
        descriptionText.DOFade(1f, openingTime).OnComplete(() =>
        {
            backgroundButton.SetListener(() => backgroundButton.transform.DOMove(new Vector3(0.00f, 7.03f, 4.80f), 0.5f).SetEase(Ease.InOutSine));
        });
    }

    protected override void SetupDefaultConfig()
    {
        fixedPosition = StartPosition;
        titleText.SetActive(true);
        descriptionText.SetActive(true, 0);
    }
}
