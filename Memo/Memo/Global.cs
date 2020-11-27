using System.Collections.Generic;
using System.Linq;
using Memo.tools;

namespace Memo
{
    class Global
    {
        private static Dictionary<uint, ThemeInfo> g_colorInfoMap;
        private static List<ThemeInfo> g_colorInfos;
        public static uint CurrentThemeId;

        public static List<ThemeInfo> ColorInfos
        {
            get
            {
                if (null != g_colorInfos)
                {
                    return g_colorInfos;
                }

                // ReSharper disable once JoinDeclarationAndInitializer
                string path;
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

                return g_colorInfos;
            }
        }

        public static uint DefaultTheme => ColorInfos[0].Id;
        public static OptionControl OptionCtrl { get; } = new OptionControl();

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