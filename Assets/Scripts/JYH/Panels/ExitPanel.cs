using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// 종료 패널 클래스
/// </summary>
[RequireComponent(typeof(Image))]
[RequireComponent(typeof(Button))]
public class ExitPanel : Panel
{
    [Header("알림 텍스트"), SerializeField]
    private TMP_Text noticeText;

    [Header("종료 텍스트"), SerializeField]
    private TMP_Text exitText;

    [Header("취소 텍스트"), SerializeField]
    private TMP_Text cancelText;

    public override void ChangeText()
    {
        base.ChangeText();
        noticeText.Set(Translation.Get(Translation.Letter.DoYouWantToQuit), tmpFontAsset);
        //exitText.Set(Translation.Get(Translation.Letter.Exit));
        //cancelText.Set(Translation.Get(Translation.Letter.Cancel));
    }

    //종료 버튼 클릭 시 호출되는 메소드
    public void Exit()
    {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
}