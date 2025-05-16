public static class English 
{
    public static void Set(ref string[] letters)
    {
        int length = letters != null ? letters.Length : 0;
        for (int i = 0; i < length; i++)
        {
            switch ((Translation.Letter)i)
            {
                case Translation.Letter.Start:
                    letters[i] = "START";
                    break;
                case Translation.Letter.Stage:
                    letters[i] = "STAGE";
                    break;
                case Translation.Letter.PVP:
                    letters[i] = "PVP";
                    break;
                case Translation.Letter.Store:
                    letters[i] = "STORE";
                    break;
                case Translation.Letter.Option:
                    letters[i] = "OPTION";
                    break;
                case Translation.Letter.Exit:
                    letters[i] = "EXIT";
                    break;
                case Translation.Letter.Music:
                    letters[i] = "MUSIC";
                    break;
                case Translation.Letter.Story:
                    letters[i] = "STORY";
                    break;
                case Translation.Letter.DoYouWantToQuit:
                    letters[i] = "Do You Want To Quit?";
                    break;
                case Translation.Letter.Select:
                    letters[i] = "SELECT";
                    break;
            }
        }
    }
}