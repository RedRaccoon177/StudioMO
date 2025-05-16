using System;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// 스테이지 선택 패널 클래스
/// </summary>
[RequireComponent(typeof(Image))]
[RequireComponent(typeof(Button))]
public class StageSelectPanel : Panel
{
    [Header("스테이지를 실행시키는 버튼"), SerializeField]
    private Button playButton;

    [Header("테마가 무엇인지 보여주는 이미지"), SerializeField]
    private Image themeImage;

    [Header("음악명이 무엇인지 알려주는 텍스트"), SerializeField]
    private TMP_Text musicText;

    [Header("이야기를 설명해주는 텍스트"), SerializeField]
    private TMP_Text storyText;

    [Header("별 활성화 스프라이트"), SerializeField]
    private Sprite enableStarSprite;

    [Header("별 비활성화 스프라이트"), SerializeField]
    private Sprite disableStarSprite;

    private static readonly int StarCount = 2;
    [Header("패널 안에 보이는 별 이미지들"), SerializeField]
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