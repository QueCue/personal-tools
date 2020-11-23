namespace Memo
{
    public class ThemeInfo
    {
        public string Guid;
        public string Desc;
        public string DisplayColor;
        public string TitleBarColor;
        public string BgColor;
        public bool IsDark;

        public ThemeInfo()
        {
            Guid = System.Guid.NewGuid().ToString();
        }

        public ThemeInfo(string desc
            , string displayColor
            , string titleBarColor
            , string bgColor
            , bool isDark = false) : this()
        {
            Desc = desc;
            DisplayColor = displayColor;
            TitleBarColor = titleBarColor;
            BgColor = bgColor;
            IsDark = isDark;
        }
    }
}
