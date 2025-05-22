using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(PlayerCtrlManager))]
public class StageManager : Manager
{
    public static readonly string SceneName = "StageScene";

    [Header(nameof(StageManager))]
    [SerializeField]
    private TMP_Text timeText;
    [SerializeField]
    private Image timeImage;
    private float timeCurrentValue = 0.0f;
    [SerializeField, Range(0, int.MaxValue)]
    private float timeMaxValue = 0.0f;

    [SerializeField]
    private TMP_Text gatheringText;
    [SerializeField]
    private Image gatheringImage;

    [SerializeField]
    private TMP_Text goalMinText;
    private uint goalMinValue = 0;

    private bool hasPlayerCtrlManager = false;

    private PlayerCtrlManager playerCtrlManager = null;

    private PlayerCtrlManager getPlayerCtrlManager {
        get
        {
            if(hasPlayerCtrlManager == false)
            {
                playerCtrlManager = GetComponent<PlayerCtrlManager>();
                hasPlayerCtrlManager = true;
            }
            return playerCtrlManager;
        }
    }

    private ObjectPoolingBullet objectPoolingBullet;

    [SerializeField]
    private Player player;

    protected override void Update()
    {
        base.Update();
        if (timeCurrentValue > 0)
        {
            timeCurrentValue -= Time.deltaTime;
            if (timeCurrentValue < 0)
            {
                timeCurrentValue = 0;   //게임 종료
            }
        }
        timeImage.Fill(timeMaxValue > 0 ? timeCurrentValue/timeMaxValue : 1);
    }

    protected override void Initialize()
    {
        StageData stageData = StageData.current;
        if(stageData != null)
        {
            GameObject gameObject = stageData.GetMapObject();
            if(gameObject != null)
            {
                Instantiate(gameObject, Vector3.zero, Quaternion.identity);
            }
            goalMinValue = stageData.GetGoalMinValue();
        }
        timeCurrentValue = timeMaxValue;
        gatheringText.Set(Translation.Get(Translation.Letter.Goal) + ": " + goalMinValue);
        gatheringImage.Fill(goalMinValue > 0 ? 0 : 1);

    }

    protected override void ChangeText()
    {

    }
}