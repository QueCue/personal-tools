using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Memo
{
    /// <summary>
    /// MemoWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MemoWindow : Window
    {
        private Window m_oriParentWnd;

        public MemoWindow()
        {
            InitializeComponent();
            MouseLeftButtonDown += (sender, e) => DragMove();
        }

        private void OnCloseClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void OnTopMostClick(object sender, RoutedEventArgs e)
        {
            Topmost = !Topmost;
            if (sender is Button btn)
            {
                var tg = btn.RenderTransform as TransformGroup;
                var tgNew = tg.CloneCurrentValue();
                if (null != tgNew)
                {
                    RotateTransform rt = tgNew.Children[2] as RotateTransform;
                    btn.RenderTransformOrigin = new Point(0.5, 0.5);
                    rt.Angle = Topmost ? 0 : 90;
                }

                btn.RenderTransform = tgNew;
            }

            Owner = Topmost ? null : m_oriParentWnd;
        }

        private void OnOptionClick(object sender, RoutedEventArgs e)
        {
        }

        private void MemoWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            m_oriParentWnd = Owner;
        }

        private void OnNewClick(object sender, RoutedEventArgs e)
        {
            (m_oriParentWnd as MainWindow).New();
        }
    }
}
