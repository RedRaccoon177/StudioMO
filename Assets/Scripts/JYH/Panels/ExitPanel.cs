using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// ���� �г� Ŭ����
/// </summary>
[RequireComponent(typeof(Image))]
[RequireComponent(typeof(Button))]
public class ExitPanel : Panel
{
    [Header("�˸� �ؽ�Ʈ"), SerializeField]
    private TMP_Text noticeText;

    [Header("���� �ؽ�Ʈ"), SerializeField]
    private TMP_Text exitText;

    [Header("��� �ؽ�Ʈ"), SerializeField]
    private TMP_Text cancelText;

    public override void ChangeText()
    {
        base.ChangeText();
        noticeText.Set(Translation.Get(Translation.Letter.DoYouWantToQuit), tmpFontAsset);
        //exitText.Set(Translation.Get(Translation.Letter.Exit));
        //cancelText.Set(Translation.Get(Translation.Letter.Cancel));
    }

    //���� ��ư Ŭ�� �� ȣ��Ǵ� �޼ҵ�
    public void Exit()
    {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
}