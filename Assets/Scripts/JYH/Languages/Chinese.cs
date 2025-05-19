public static class Chinese
{
    public static void Set(ref string[] letters)
    {
        int length = letters != null ? letters.Length : 0;
        for (int i = 0; i < length; i++)
        {
            switch ((Translation.Letter)i)
            {
                case Translation.Letter.Start:
                    letters[i] = "开始";
                    break;
                case Translation.Letter.Stage:
                    letters[i] = "";
                    break;
                case Translation.Letter.PVP:
                    letters[i] = "";
                    break;
                case Translation.Letter.Store:
                    letters[i] = "";
                    break;
                case Translation.Letter.Option:
                    letters[i] = "";
                    break;
                case Translation.Letter.Exit:
                    letters[i] = "";
                    break;
                case Translation.Letter.Music:
                    letters[i] = "";
                    break;
                case Translation.Letter.Story:
                    letters[i] = "";
                    break;
                case Translation.Letter.DoYouWantToQuit:
                    letters[i] = "要结束游戏吗?";
                    break;
                case Translation.Letter.Select:
                    letters[i] = "";
                    break;
            }
        }
    }
}
