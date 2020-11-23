using System.Collections.Generic;

namespace Memo
{
    class Theme
    {
        public static List<ThemeInfo> ColorInfos => new List<ThemeInfo>()
        {
            new ThemeInfo("黄色", "#FFFFE66E", "#FFFFF2AB", "#FFFFF7D1"),
            new ThemeInfo("绿色", "#FFA1EF9B", "#FFCBF1C4", "#FFE4F9E0"),
            new ThemeInfo("粉色", "#FFFFAFDF", "#FFFFCCE5", "#FFFFE4F1"),
            new ThemeInfo("紫色", "#FFD7AFFF", "#FFE7CFFF", "#FFF2E6FF"),
            new ThemeInfo("蓝色", "#FF9EDFFF", "#FFCDE9FF", "#FFE2F1FF"),
            new ThemeInfo("灰色", "#FFE0E0E0", "#FFE1DFDD", "#FFF3F2F1"),
            new ThemeInfo("炭笔", "#FF767676", "#FF494745", "DimGray", true),
        };

        public static ThemeInfo Default => ColorInfos[0];
    }
}
