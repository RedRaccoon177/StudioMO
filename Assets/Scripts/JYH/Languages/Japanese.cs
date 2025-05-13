public static class Japanese
{
    public static void Set(ref string[] letters)
    {
        int length = letters != null ? letters.Length : 0;
        for (int i = 0; i < length; i++)
        {
            switch ((Translation.Letter)i)
            {
                case Translation.Letter.Start:
                    letters[i] = "������";
                    break;
                case Translation.Letter.Stage:
                    letters[i] = "��������";
                    break;
                case Translation.Letter.Match:
                    letters[i] = "����";
                    break;
                case Translation.Letter.Mode:
                    letters[i] = "���";
                    break;
                case Translation.Letter.Shop:
                    letters[i] = "����";
                    break;
                case Translation.Letter.Option:
                    letters[i] = "�ɼ�";
                    break;
                case Translation.Letter.Exit:
                    letters[i] = "������";
                    break;
            }
        }
    }
}
