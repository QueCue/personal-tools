using System;
using System.Windows.Media;

namespace Memo.tools
{
    public static class Tools
    {
        public static Color ColorFromString(string str)
        {
            try
            {
                return (Color)ColorConverter.ConvertFromString(str);
            }
            catch (Exception)
            {
                return Colors.Black;
            }
        }
    }
}
