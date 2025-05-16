using System;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// �������� ���� �г� Ŭ����
/// </summary>
[RequireComponent(typeof(Image))]
[RequireComponent(typeof(Button))]
public class StageSelectPanel : Panel
{
    [Header("���������� �����Ű�� ��ư"), SerializeField]
    private Button playButton;

    [Header("�׸��� �������� �����ִ� �̹���"), SerializeField]
    private Image themeImage;

    [Header("���Ǹ��� �������� �˷��ִ� �ؽ�Ʈ"), SerializeField]
    private TMP_Text musicText;

    [Header("�̾߱⸦ �������ִ� �ؽ�Ʈ"), SerializeField]
    private TMP_Text storyText;

    [Header("�� Ȱ��ȭ ��������Ʈ"), SerializeField]
    private Sprite enableStarSprite;

    [Header("�� ��Ȱ��ȭ ��������Ʈ"), SerializeField]
    private Sprite disableStarSprite;

    private static readonly int StarCount = 2;
    [Header("�г� �ȿ� ���̴� �� �̹�����"), SerializeField]
    private Image[] starImages = new Image[StarCount];

    private StageData stageData;

#if UNITY_EDITOR

    private void OnValidate()
    {
        ExtensionMethod.Sort(ref starImages, StarCount);
    }
#endif

    public void Initialize(Action action)
    {
        playButton.SetListener(() =>
        {
            if (stageData != null)
            {
                StageData.current = stageData;
                action?.Invoke();
            }
        });
    }

    public void SetThemeImage(Sprite sprite)
    {
        themeImage.Set(sprite);
    }

    public void Open(StageData stageData, bool clear)
    {
        this.stageData = stageData;
        for (int i = 0; i < starImages.Length; i++)
        {
            if (i == 0 || clear == true)
            {
                starImages[i].Set(enableStarSprite);
            }
            else
            {
                starImages[i].Set(disableStarSprite);
            }
        }
        Open();
    }

    public override void Close()
    {
        stageData = null;
        for (int i = 0; i < starImages.Length; i++)
        {
            starImages[i].Set(disableStarSprite);
        }
        base.Close();
    }

    public override void ChangeText()
    {
        StringBuilder musicStringBuilder = new StringBuilder(Translation.Get(Translation.Letter.Music) + ":");
        StringBuilder storyStringBuilder = new StringBuilder(Translation.Get(Translation.Letter.Story) + ":");
        if (stageData != null)
        {
            musicStringBuilder.Append(" " + stageData.GetMusicText(Translation.language));
            storyStringBuilder.Append(" " + stageData.GetStoryText(Translation.language));
        }
        musicText.Set(musicStringBuilder.ToString());
        storyText.Set(storyStringBuilder.ToString());
    }
}