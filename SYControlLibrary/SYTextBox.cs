using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SYControlLibrary
{
    public partial class SYTextBox : TextBox
    {
        static SYTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SYTextBox), new FrameworkPropertyMetadata(typeof(SYTextBox)));
        }

        public static readonly DependencyProperty FocusedBackgroundProperty =
            DependencyProperty.Register("FocusedBackground", typeof(Brush), typeof(SYTextBox), new PropertyMetadata(Brushes.White));

        public Brush FocusedBackground
        {
            get { return (Brush)GetValue(FocusedBackgroundProperty); }
            set { SetValue(FocusedBackgroundProperty, value); }
        }
    }
}
