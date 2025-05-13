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
                    letters[i] = "스테이지";
                    break;
                case Translation.Letter.Match:
                    letters[i] = "대전";
                    break;
                case Translation.Letter.Mode:
                    letters[i] = "모드";
                    break;
                case Translation.Letter.Shop:
                    letters[i] = "상점";
                    break;
                case Translation.Letter.Option:
                    letters[i] = "옵션";
                    break;
                case Translation.Letter.Exit:
                    letters[i] = "나가기";
                    break;
            }
        }
    }
}
