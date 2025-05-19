
public static class Korean
{
    public static void Set(ref string[] letters)
    {
        int length = letters != null ? letters.Length : 0;
        for (int i = 0; i < length; i++)
        {
            switch ((Translation.Letter)i)
            {
                case Translation.Letter.Start:
                    letters[i] = "����";
                    break;
                case Translation.Letter.Stage:
                    letters[i] = "�������� ���";
                    break;
                case Translation.Letter.PVP:
                    letters[i] = "���� ���";
                    break;
                case Translation.Letter.Store:
                    letters[i] = "����";
                    break;
                case Translation.Letter.Option:
                    letters[i] = "�ɼ�";
                    break;
                case Translation.Letter.Exit:
                    letters[i] = "������";
                    break;
                case Translation.Letter.Music:
                    letters[i] = "���Ǹ�";
                    break;
                case Translation.Letter.Story:
                    letters[i] = "���丮";
                    break;
                case Translation.Letter.DoYouWantToQuit:
                    letters[i] = "������ \n���� �Ͻðڽ��ϱ�?";
                    break;
                case Translation.Letter.Select:
                    letters[i] = "����";
                    break;
            }
        }
    }
}