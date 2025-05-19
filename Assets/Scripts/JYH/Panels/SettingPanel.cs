using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;

/// <summary>
/// ���� �г� Ŭ����
/// </summary>
[RequireComponent(typeof(Image))]
[RequireComponent(typeof(Button))]
[RequireComponent(typeof(Mask))]
public class SettingPanel : Panel
{
    [Header("����� �ͼ�"), SerializeField]
    private AudioMixer audioMixer;

    [Header("����"), SerializeField]
    private TMP_Text settingText;

    [Header("�׷���"), SerializeField]
    private TMP_Text graphicText;

    [SerializeField]
    private TMP_Text backgroundSoundText;
    [SerializeField]
    private Slider backgroundSoundSlider;
    [SerializeField]
    private TMP_Text effectSoundText;
    [SerializeField]
    private Slider effectSoundSlider;
    [SerializeField]
    private TMP_Text languageText;
    [SerializeField]
    private TMP_Text detailOptionText;


    private Action<int> languageAction = null;

    private static readonly string BackgroundMixer = "Background";
    private static readonly string EffectMixer = "Effect";

    public override void Open()
    {
        if(audioMixer != null)
        {
            if(audioMixer.GetFloat(BackgroundMixer, out float background) == true && backgroundSoundSlider != null)
            {
                backgroundSoundSlider.value = background;
            }
            if (audioMixer.GetFloat(EffectMixer, out float effect) == true && effectSoundSlider != null)
            {
                effectSoundSlider.value = effect;
            }
        }
        base.Open();
    }

    public override void ChangeText()
    {
        base.ChangeText();

    }

    public void Initialize(Action<int> action)
    {
        languageAction = action;
    }

    public void SetEffectVolume(float volume)
    {
        if (audioMixer != null)
        {
            audioMixer.SetFloat(EffectMixer, volume);
        }
    }

    public void SetBackgroundVolume(float volume)
    {
        if (audioMixer != null)
        {
            audioMixer.SetFloat(BackgroundMixer, volume);
        }
    }

    public void SetLanguage(int index)
    {
        languageAction?.Invoke(index);
    }
}