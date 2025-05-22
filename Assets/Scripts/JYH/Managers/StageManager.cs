using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

[RequireComponent(typeof(BulletPatternLoader))]
public class StageManager : Manager
{
    public static readonly string SceneName = "StageScene";

    [Header(nameof(StageManager))]
    [SerializeField]
    private Player player;                                      //������ �÷��̾�

    private bool hasBulletPatternLoader = false;

    private BulletPatternLoader bulletPatternLoader = null;     //ź�� ������

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
    private AudioSource audioSource;                            //�������

    [Header("����Ű")]
    [SerializeField]
    private InputActionReference leftInputAction;               //Left Input�� ����ϱ� ���� ����
    [SerializeField]
    private InputActionReference rightInputAction;              //Right Input�� ����ϱ� ���� ����
    [SerializeField]
    private InputActionReference moveInputAction;               //Move Input�� ����ϱ� ���� ����

    [Header("������")]
    [SerializeField]
    private Transform leftHandTransform;
    [SerializeField]
    private Vector3 leftHandOffset;
    [SerializeField]
    private Transform rightHandTransform;
    [SerializeField]
    private Vector3 rightHandOffset;

    [Header("���� �ð�")]
    [SerializeField]
    private TMP_Text currentTimeText;                           //���� �ð� �ؽ�Ʈ
    [SerializeField]
    private Image currentTimeImage;                             //���� �ð� �����̵�� �̹���
    private float currentTimeValue = 0.0f;                      //���� �ð� ��
    [SerializeField, Range(0, int.MaxValue)]
    private float limitTimeValue = 0.0f;                        //���� �ð� ��

    [Header("���� ȹ�� ����")]
    [SerializeField]
    private TMP_Text currentMineralText;                        //���� ���� �ؽ�Ʈ
    [SerializeField]
    private Image currentMineralImage;                          //���� ���� �����̵�� �̹���
    [SerializeField]
    private TMP_Text goalMineralText;                           //��ǥ ���� �ؽ�Ʈ
    [SerializeField]
    private uint goalMineralValue = 0;                          //��ǥ ���� ��

    protected override void Update()
    {
        if (player != null)
        {
            if(Camera.main != null)
            {
                player.UpdateHead(Camera.main.transform.rotation);
            }
            if (leftHandTransform != null)
            {
                player.UpdateLeftHand(leftHandTransform.position + leftHandOffset, leftHandTransform.rotation);
            }
            if (rightHandTransform != null)
            {
                player.UpdateRightHand(rightHandTransform.position + rightHandOffset, rightHandTransform.rotation);
            }
            fixedPosition = player.transform.position + CameraOffsetPosition;
        }
        base.Update();
        if (currentTimeValue > 0)
        {
            currentTimeValue -= Time.deltaTime;
            if (currentTimeValue < 0)
            {
                currentTimeValue = 0;   //���� ����
            }
        }
        currentTimeText.Set(currentTimeValue.ToString());
        currentTimeImage.Fill(limitTimeValue > 0 ? currentTimeValue/limitTimeValue : 1);
    }

    public override void OnEnable()
    {
        base.OnEnable();
        leftInputAction.Set(true, OnLeftSelect, OnLeftSelect);
        rightInputAction.Set(true, OnRightSelect, OnRightSelect);
        moveInputAction.Set(true, OnMoveSelect, OnMoveSelect);
    }

    public override void OnDisable()
    {
        base.OnDisable();
        leftInputAction.Set(false, OnLeftSelect, OnLeftSelect);
        rightInputAction.Set(false, OnRightSelect, OnRightSelect);
        moveInputAction.Set(false, OnMoveSelect, OnMoveSelect);
    }

    [SerializeField]
    private StageData test;

    protected override void Initialize()
    {
        if (player != null)
        {
            fixedPosition = player.transform.position + CameraOffsetPosition;
        }
        else
        {
            fixedPosition = CameraOffsetPosition;
        }
        StageData stageData = test;         //StageData stageData = StageData.current;
        if (stageData != null)
        {
            GameObject gameObject = stageData.GetMapObject();
            if(gameObject != null)
            {
                Instantiate(gameObject, Vector3.zero, Quaternion.identity);
            }
            goalMineralValue = stageData.GetGoalMinValue();
            TextAsset bulletTextAsset = stageData.GetBulletTextAsset();
            getBulletPatternLoader.SetCSVData(bulletTextAsset);
            if (audioSource != null)
            {
                AudioClip audioClip = stageData.GetAudioClip();
                if (audioClip != null)
                {
                    audioSource.clip = audioClip;
                    limitTimeValue = audioClip.length;
                    audioSource.Play();
                }
            }
        }
        currentTimeValue = limitTimeValue;
        SetCurrentGathering(0);
    }

    protected override void ChangeText()
    {
        goalMineralText.Set(Translation.Get(Translation.Letter.Goal) + ": " + goalMineralValue);
    }

    private void OnLeftSelect(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
        }
        else if (context.canceled)
        {
        }
    }

    private void OnRightSelect(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
        }
        else if (context.canceled)
        {
        }
    }

    private void OnMoveSelect(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            player?.UpdateMove(context.ReadValue<Vector2>());
        }
        else if (context.canceled)
        {
            player?.UpdateMove(Vector2.zero);
        }
    }

    private void SetCurrentGathering(uint value)
    {
        currentMineralText.Set(value.ToString());
        currentMineralImage.Fill(goalMineralValue > 0 ? (float)value / goalMineralValue : 1);
    }
}