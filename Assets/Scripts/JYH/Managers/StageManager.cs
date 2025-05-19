using UnityEngine;

public class StageManager : Manager
{
    public static readonly string SceneName = "StageScene";

    [Header(nameof(StageManager))]
    [SerializeField, Range(0, int.MaxValue)]
    private float remainingTime = 0.0f;

    protected override void Update()
    {
        base.Update();
        if(remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
        }
    }

    protected override void Initialize()
    {
    }

    protected override void ChangeText()
    {
    }
}