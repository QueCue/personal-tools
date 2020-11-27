using System.Linq;
using System.Windows;
using System.Windows.Documents;

namespace Memo
{
    public partial class MemoWindow
    {
        private TextSelection Selection => m_mainInput.Selection;

        private TextRange GetFirstSelection()
        {
            TextPointer endPos = Selection.Start.GetPositionAtOffset(1);
            return null == endPos ? null : new TextRange(Selection.Start, endPos);
        }

        private void HandleText(DependencyProperty formattingProperty, object targetValue, object defaultValue)
        {
            TextRange firstSelection = GetFirstSelection();
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
            this.SetDecorations(new TextRange(Selection.Start, Selection.End),
                TextDecorations.Underline);
            return;
            HandleText(Inline.TextDecorationsProperty, TextDecorations.Underline, null);
        }

        private void StrikeThroughBtn_Click(object sender, RoutedEventArgs e)
        {
            //HandleText(Inline.TextDecorationsProperty, TextDecorations.Strikethrough, null);
            this.SetDecorations(new TextRange(Selection.Start, Selection.End),
                TextDecorations.Strikethrough);
        }

        private void SetDecorations(TextRange textRange, TextDecorationCollection decoration)
        {
            var decorations = textRange.GetPropertyValue(Inline.TextDecorationsProperty) as TextDecorationCollection
                              ?? new TextDecorationCollection();

            decorations = decorations.Contains(decoration.First())
                ? new TextDecorationCollection(decorations.Except(decoration))
                : new TextDecorationCollection(decorations.Union(decoration));

            textRange.ApplyPropertyValue(Inline.TextDecorationsProperty, decorations);
        }
    }
}