public static class Japanese
{
    public static void Set(ref string[] letters)
    {
        int length = letters != null ? letters.Length : 0;
        for (int i = 0; i < length; i++)
        {
            switch ((Translation.Letter)i)
            {
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
                case Translation.Letter.Goal:
                    letters[i] = "";
                    break;
                case Translation.Letter.Story:
                    letters[i] = "";
                    break;
                case Translation.Letter.DoYouWantToQuit:
                    letters[i] = "ゲームを終わらせるんですか?";
                    break;
                case Translation.Letter.Select:
                    letters[i] = "";
                    break;
            }
        }
    }
}
