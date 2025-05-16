using System;

public static class Translation
{
    public enum Language : byte
    {
        English,
        Korean,
        Chinese,
        Japanese,
    }

    public static int count
    {
        get
        {
            return Enum.GetNames(typeof(Language)).Length;
        }
    }

    public enum Letter: byte
    {
        Start,              //����
        Stage,              //�������� ���
        PVP,                //���� ���
        Store,              //����
        Option,             //�ɼ�
        Exit,               //������
        Music,              //���Ǹ�
        Story,              //���丮
        DoYouWantToQuit,    //������ ���� �Ͻðڽ��ϱ�?
        Select,             //����
        End
    }

    private static string[] letters = new string[(int)Letter.End];

    public static Language language
    {
        private set;
        get;
    }

    public static void Set(Language language)
    {
        Translation.language = language;
        switch(Translation.language)
        {
            case Language.English:
                English.Set(ref letters);
                break;
            case Language.Korean:
                Korean.Set(ref letters);
                break;
            case Language.Chinese:
                Chinese.Set(ref letters);
                break;
            case Language.Japanese:
                Japanese.Set(ref letters);
                break;
        }
    }

    public static string Get(Letter letter)
    {
        if (letter >= Letter.Start && letter < Letter.End)
        {
            return letters[(int)letter];
        }
        return null;
    }
}