using UnityEngine;

public class StageManager : Manager
{
    public static readonly string SceneName = "StageScene";

    [Header(nameof(StageManager))]
    [SerializeField]
    private Transform leftHandTransform;
    [SerializeField]
    private Transform rightHandTransform;
    [SerializeField, Range(0, int.MaxValue)]
    private float remainingTime = 0.0f;

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
        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
        }
    }

    protected override void Initialize()
    {
        Camera camera = Camera.main;
        if(camera != null)
        {
            Debug.Log(camera.cullingMask);
        }
    }

    protected override void ChangeText()
    {
    }
}