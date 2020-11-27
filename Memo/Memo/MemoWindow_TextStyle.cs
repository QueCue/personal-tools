using System.Linq;
using System.Windows;
using System.Windows.Documents;

namespace Memo
{
    public partial class MemoWindow : Window
    {
        private void BoldBtn_Click(object sender, RoutedEventArgs e)
        {
            m_mainInput.Selection.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Bold);
        }

        private void ItalicBtn_Click(object sender, RoutedEventArgs e)
        {
            var selection = m_mainInput.Selection;
            var endPos = selection.Start.GetPositionAtOffset(1);
            if (null == endPos)
            {
                return;
            }

            m_mainInput.Selection.Select(selection.Start, endPos);
            //m_mainInput.Focus();
            return;
            //var obj = .GetPropertyValue(TextElement.FontStyleProperty);
            //if (null == obj
            //    || (obj is FontStyle fontStyle && fontStyle != FontStyles.Italic))
            //{
            //    selection.ApplyPropertyValue(TextElement.FontStyleProperty, FontStyles.Italic);
            //}
            //else
            //{
            //    selection.ApplyPropertyValue(TextElement.FontStyleProperty, FontStyles.Normal);
            //}
            //m_mainInput.Focus();
        }

        private void UnderlineBtn_Click(object sender, RoutedEventArgs e)
        {
            var selection = m_mainInput.Selection;
            if (!selection.IsEmpty)
            {
                var tdc = (TextDecorationCollection)selection.GetPropertyValue(Inline.TextDecorationsProperty);
                if (tdc == null || !tdc.SequenceEqual(TextDecorations.Underline))
                {
                    selection.ApplyPropertyValue(Inline.TextDecorationsProperty, TextDecorations.Underline);
                }
                else
                {
                    selection.ApplyPropertyValue(Inline.TextDecorationsProperty, null);
                }
            }

            //m_mainInput.Focus();
        }

        private void StrikeThroughBtn_Click(object sender, RoutedEventArgs e)
        {
            var selection = m_mainInput.Selection;
            if (!selection.IsEmpty)
            {
                var tdc = (TextDecorationCollection)selection.GetPropertyValue(Inline.TextDecorationsProperty);
                if (tdc == null || !tdc.SequenceEqual(TextDecorations.Strikethrough))
                {
                    selection.ApplyPropertyValue(Inline.TextDecorationsProperty, TextDecorations.Strikethrough);
                }
                else
                {
                    selection.ApplyPropertyValue(Inline.TextDecorationsProperty, null);
                }
            }

            //m_mainInput.Focus();
        }
    }
}
