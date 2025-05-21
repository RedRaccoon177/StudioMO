using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
    private TMP_Text goalText;
    [SerializeField, Range(0, int.MaxValue)]
    private uint goalMinValue = 50;

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
        }
        timeCurrentValue = timeMaxValue;
        SetCurrentGathering(0);
    }

    protected override void ChangeText()
    {

    }

    private void SetCurrentGathering(uint value)
    {
        gatheringImage.Fill(goalMinValue > 0 ? value / goalMinValue : 1);
    }
}