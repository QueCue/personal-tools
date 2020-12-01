using System.Windows;
using System.Windows.Documents;

namespace Memo
{
    public partial class MemoWindow
    {
        private TextSelection Selection => m_mainInput.Selection;

        private TextRange GetFirstSelection(TextRange range)
        {
            TextPointer endPos = range.Start.GetPositionAtOffset(1, LogicalDirection.Forward);
            return null == endPos ? null : new TextRange(range.Start, endPos);
        }

        private void HandleText(DependencyProperty formattingProperty, object targetValue, object defaultValue)
        {
            TextRange firstSelection = GetFirstSelection(Selection);
            if (null == firstSelection)
            {
                return;
            }

            object obj = firstSelection.GetPropertyValue(formattingProperty);
            if (null == obj
                || obj.GetType() == targetValue.GetType()
                && !obj.Equals(targetValue))
            {
                Selection.ApplyPropertyValue(formattingProperty, targetValue);
            }
            else
            {
                Selection.ApplyPropertyValue(formattingProperty, defaultValue);
            }
        }

        private void BoldBtn_Click(object sender, RoutedEventArgs e)
        {
            HandleText(TextElement.FontWeightProperty, FontWeights.Bold, FontWeights.Normal);
        }

        private void ItalicBtn_Click(object sender, RoutedEventArgs e)
        {
            HandleText(TextElement.FontStyleProperty, FontStyles.Italic, FontStyles.Normal);
        }

        private void UnderlineBtn_Click(object sender, RoutedEventArgs e)
        {
            var textRange = new TextRange(Selection.Start, Selection.End);
            SetDecorations(textRange, TextDecorations.Underline);
            return;
            HandleText(Inline.TextDecorationsProperty, TextDecorations.Underline, null);
        }

        private void StrikeThroughBtn_Click(object sender, RoutedEventArgs e)
        {
            //HandleText(Inline.TextDecorationsProperty, TextDecorations.Strikethrough, null);
            var textRange = new TextRange(Selection.Start, Selection.End);
            SetDecorations(textRange, TextDecorations.Strikethrough);
        }

        private void SetDecorations(TextRange textRange, TextDecorationCollection decoration)
        {
            //var decorations = textRange.GetPropertyValue(Inline.TextDecorationsProperty) as TextDecorationCollection
            //           ?? new TextDecorationCollection();
            //decorations = decorations.Contains(decoration.First())
            //    ? new TextDecorationCollection(decorations.Except(decoration))
            //    : new TextDecorationCollection(decorations.Union(decoration));

            //textRange.ApplyPropertyValue(Inline.TextDecorationsProperty, decorations);

            //return;

            TextPointer startPos = textRange.Start;
            TextPointer endPos = startPos.GetPositionAtOffset(1, LogicalDirection.Forward);
            while (startPos.CompareTo(textRange.End) != 0)
            {
                var tmp = new TextRange(startPos, endPos);
                //var dd = tmp.GetPropertyValue(Inline.TextDecorationsProperty) as TextDecorationCollection
                //             ?? new TextDecorationCollection();
                //dd = dd.Contains(decoration.First())
                //    ? new TextDecorationCollection(dd.Except(decoration))
                //    : new TextDecorationCollection(dd.Union(decoration));

                object asf = tmp.GetPropertyValue(Inline.TextDecorationsProperty);

                // var dd = new TextDecorationCollection() { TextDecorations.Strikethrough, TextDecorations.Underline };
                //
                // tmp.ApplyPropertyValue(Inline.TextDecorationsProperty, dd);
                startPos = startPos.GetNextInsertionPosition(LogicalDirection.Forward);
                endPos = startPos.GetNextInsertionPosition(LogicalDirection.Forward);
            }
        }
    }
}