using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(PlayerCtrlManager))]
[RequireComponent(typeof(BulletPatternLoader))]
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

    private bool hasBulletPatternLoader = false;

    private BulletPatternLoader bulletPatternLoader = null;

    private BulletPatternLoader getBulletPatternLoader {
        get
        {
            if (hasBulletPatternLoader == false)
            {
                bulletPatternLoader = GetComponent<BulletPatternLoader>();
                hasBulletPatternLoader = true;
            }
            return bulletPatternLoader;
        }
    }   

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
                timeCurrentValue = 0;   //���� ����
            }
        }
        timeText.Set(timeCurrentValue.ToString());
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
            TextAsset bulletTextAsset = stageData.GetBulletTextAsset();
            getBulletPatternLoader.SetCSVData(bulletTextAsset);
        }
        timeCurrentValue = timeMaxValue;
        SetCurrentGathering(0);
    }

    protected override void ChangeText()
    {
        goalMinText.Set(Translation.Get(Translation.Letter.Goal) + ": " + goalMinValue);
    }

    private void SetCurrentGathering(uint value)
    {
        gatheringText.Set(value.ToString());
        gatheringImage.Fill(goalMinValue > 0 ? (float)value / goalMinValue : 1);
    }
}