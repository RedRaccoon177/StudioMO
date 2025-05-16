
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
                case Translation.Letter.Music:
                    letters[i] = "음악명";
                    break;
                case Translation.Letter.Story:
                    letters[i] = "스토리";
                    break;
                case Translation.Letter.DoYouWantToQuit:
                    letters[i] = "게임을 \n종료 하시겠습니까?";
                    break;
                case Translation.Letter.Select:
                    letters[i] = "선택";
                    break;
            }
        }
    }
}