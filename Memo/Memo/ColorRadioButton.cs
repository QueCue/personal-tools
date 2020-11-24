using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Memo
{
    public class ColorRadioButton : RadioButton
    {
        static ColorRadioButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ColorRadioButton), new FrameworkPropertyMetadata(typeof(ColorRadioButton)));
        }

        public static readonly DependencyProperty MarkStrokeProperty =
           DependencyProperty.Register("MarkStroke", typeof(Brush), typeof(ColorRadioButton), new PropertyMetadata(Brushes.White));

        public Brush MarkStroke
        {
            get => (Brush)GetValue(MarkStrokeProperty);
            set => SetValue(MarkStrokeProperty, value);
        }
    }
}
