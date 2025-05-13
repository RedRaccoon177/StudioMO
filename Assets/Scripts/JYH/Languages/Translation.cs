public static class Translation
{
    public enum Language : byte
    {
        Korean,
        English,
        Japanese,
        Chinese,
    }

    public enum Letter: byte
    {
        Start,
        Stage,
        Match,
        Mode,
        Shop,
        Option,
        Exit,
        End
    }

    private static string[] letters = new string[(int)Letter.End];

    public static void Set(Language language)
    {
        switch(language)
        {
            case Language.Korean:
                Korean.Set(ref letters);
                break;
            case Language.English:
                English.Set(ref letters);
                break;
            case Language.Japanese:
                Japanese.Set(ref letters);
                break;
            case Language.Chinese:
                Chinese.Set(ref letters);
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