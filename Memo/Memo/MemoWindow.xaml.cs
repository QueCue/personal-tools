using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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
            //SYTEST
            OnTopMostClick(null, null);
            Closed += (sender, e) => Application.Current.Shutdown();
        }

        private void OnCloseClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void OnTopMostClick(object sender, RoutedEventArgs e)
        {
            Topmost = !Topmost;
            var tg = m_topmostBtn.RenderTransform as TransformGroup;
            var tgNew = tg.CloneCurrentValue();
            if (null != tgNew)
            {
                RotateTransform rt = tgNew.Children[2] as RotateTransform;
                m_topmostBtn.RenderTransformOrigin = new Point(0.5, 0.5);
                rt.Angle = Topmost ? 0 : 90;
            }

            m_topmostBtn.RenderTransform = tgNew;
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
            (m_oriParentWnd as MainWindow).NewMemo();
        }

        private void OnTopmostBtnTipOpen(object sender, ToolTipEventArgs e)
        {
            m_topmostBtn.ToolTip = Topmost ? "取消置顶" : "置顶";
        }

        private void TitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                WindowState = WindowState == WindowState.Maximized
                    ? WindowState.Normal : WindowState.Maximized;
            }
        }

        private void TitleBar_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed)
            {
                return;
            }

            if (WindowState != WindowState.Normal)
            {
                WindowState = WindowState.Normal;
                Point mousePos = Mouse.GetPosition(e.Source as FrameworkElement);
                Left = mousePos.X - Width * 0.5;
                Top = 0;
            }

            DragMove();
        }
    }
}
