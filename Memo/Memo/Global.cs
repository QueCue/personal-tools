using Memo.tools;
using System.Collections.Generic;

namespace Memo
{
    class Global
    {
        private static List<ThemeInfo> g_colorInfos;

        public static List<ThemeInfo> ColorInfos
        {
            get
            {
                if (null == g_colorInfos)
                {
                    string path = string.Empty;
#if DEBUG
                    path = "../../config/Theme.json";
#else
                    path = "config/Theme.json";
#endif
                    g_colorInfos = JsonHelper.ReadJson<List<ThemeInfo>>(path);
                }

                return g_colorInfos;
            }
        }

        public static ThemeInfo Default => ColorInfos[0];

        private static OptionWindow m_optionWnd = new OptionWindow();
        public static OptionWindow OptionWnd => m_optionWnd;
    }
}
