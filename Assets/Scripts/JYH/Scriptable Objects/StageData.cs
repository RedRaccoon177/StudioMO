using System;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(StageData), menuName = "Scriptable Object/" + nameof(StageData), order = 0)]
public class StageData : ScriptableObject
{
    public static StageData current = null;

    [Serializable]
    private struct Text
    {
        [SerializeField]
        private string korean;
        [SerializeField]
        private string english;
        [SerializeField]
        private string japanese;
        [SerializeField]
        private string chinese;

        public string Get(Translation.Language language)
        {
            switch (language)
            {
                case Translation.Language.Korean:
                    return korean;
                case Translation.Language.English:
                    return english;
                case Translation.Language.Japanese:
                    return japanese;
                case Translation.Language.Chinese:
                    return chinese;
                default:
                    return null;
            }
        }
    }

    [Header("¿Ωæ«∏Ì"), SerializeField]
    private Text musicText;

    [Header("Ω∫≈‰∏Æ"), SerializeField]
    private Text storyText;

    [Header("πË∞Ê¿Ωæ«"), SerializeField]
    private AudioClip audioClip;

    [Header("∏ "), SerializeField]
    private GameObject map;

    public string GetMusicText(Translation.Language language)
    {
        return musicText.Get(language);
    }

    public string GetStoryText(Translation.Language language)
    {
        return storyText.Get(language);
    }
}