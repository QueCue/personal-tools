using Memo.tools;
using System.Collections.Generic;
using System.Linq;

namespace Memo
{
    class Global
    {
        private static Dictionary<uint, ThemeInfo> g_colorInfoMap;
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
                    if (null == g_colorInfos)
                    {
                        return null;
                    }

                    g_colorInfoMap = g_colorInfos.ToDictionary(info => info.Id);
                }

                return g_colorInfos;
            }
        }

        public static ThemeInfo DefaultTheme => ColorInfos[0];

        private static OptionControl m_optionCtrl = new OptionControl();
        public static OptionControl OptionCtrl => m_optionCtrl;

        public static ThemeInfo GetThemeInfo(uint id)
        {
            if (null != g_colorInfoMap
                && g_colorInfoMap.TryGetValue(id, out ThemeInfo info))
            {
                return info;
            }

            return null;
        }
    }
}
