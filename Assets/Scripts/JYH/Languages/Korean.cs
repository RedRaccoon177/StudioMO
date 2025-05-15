
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
                    letters[i] = "시작";
                    break;
                case Translation.Letter.Stage:
                    letters[i] = "스테이지 모드";
                    break;
                case Translation.Letter.PVP:
                    letters[i] = "대전 모드";
                    break;
                case Translation.Letter.Store:
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