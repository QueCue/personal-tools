using System;

namespace Memo
{
    [Serializable]
    public class ThemeInfo
    {
        public uint Id;
        public string Desc;
        public string DisplayColor;
        public string TitleBarColor;
        public string BgColor;
        public bool IsDark;

        public ThemeInfo() { }

        public ThemeInfo(uint id, string desc, string displayColor
            , string titleBarColor, string bgColor, bool isDark = false)
        {
            Id = id;
            Desc = desc;
            DisplayColor = displayColor;
            TitleBarColor = titleBarColor;
            BgColor = bgColor;
            IsDark = isDark;
        }
    }
}
