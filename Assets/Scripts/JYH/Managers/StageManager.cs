using UnityEngine;
using UnityEngine.UI;

public class StageManager : Manager
{
    public static readonly string SceneName = "StageScene";

    [Header(nameof(StageManager))]
    [SerializeField]
    private Transform leftHandTransform;
    [SerializeField]
    private Transform rightHandTransform;

    [SerializeField]
    private ObjectPoolingBullet objectPoolingBullet;

    [Header("¹è°æÀ½¾Ç")]
    [SerializeField]
    private Image timeLimitImage;
    private float timeLimitCurrentValue = 0.0f;
    private float timeLimitMaxValue = 0.0f;

    [SerializeField]
    private Player player;

    protected override void Update()
    {
        base.Update();
        if (player != null)
        {
            if (Camera.main != null)
            {
                player.UpdateMove(Camera.main.transform.position, Camera.main.transform.rotation);
            }
            if (leftHandTransform != null)
            {
                player.UpdateLeftHand(leftHandTransform.position, leftHandTransform.rotation);
            }
            if (rightHandTransform != null)
            {
                player.UpdateRightHand(rightHandTransform.position, rightHandTransform.rotation);
            }
        }
        if (timeLimitMaxValue > 0)
        {
            timeLimitMaxValue -= Time.deltaTime;
        }
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
    }

    protected override void ChangeText()
    {
    }
}