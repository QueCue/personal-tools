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
                return (Color) ColorConverter.ConvertFromString(str);
            }
            catch (Exception)
            {
                return Colors.Black;
            }
        }


        // Enumerate all the descendants of the visual object.
        public static void EnumVisual(Visual myVisual, Action<Visual> action)
        {
            for (var i = 0; i < VisualTreeHelper.GetChildrenCount(myVisual); i++)
            {
                // Retrieve child visual at specified index value.
                var childVisual = (Visual) VisualTreeHelper.GetChild(myVisual, i);
                // Do processing of the child visual object.
                action?.Invoke(myVisual);
                // Enumerate children of the child visual object.
                EnumVisual(childVisual, action);
            }
        }
    }
}