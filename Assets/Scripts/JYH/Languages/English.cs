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
                    letters[i] = "Start";
                    break;
                case Translation.Letter.Stage:
                    letters[i] = "Stage";
                    break;
                case Translation.Letter.Match:
                    letters[i] = "Match";
                    break;
                case Translation.Letter.Mode:
                    letters[i] = "Mode";
                    break;
                case Translation.Letter.Shop:
                    letters[i] = "Shop";
                    break;
                case Translation.Letter.Option:
                    letters[i] = "Option";
                    break;
                case Translation.Letter.Exit:
                    letters[i] = "Exit";
                    break;
            }
        }
    }
}