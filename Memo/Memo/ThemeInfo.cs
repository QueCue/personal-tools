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
    }
}
