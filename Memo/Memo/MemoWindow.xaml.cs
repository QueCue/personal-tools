using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Memo
{
    /// <summary>
    /// MemoWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MemoWindow : Window
    {
        private Window m_oriParentWnd;
        private double m_unfoldWndHeight;
        private ThicknessAnimation m_titleBarAnim;
        private readonly static Thickness g_titleNormalMargin = new Thickness(0, 0, 0, 32);

        public MemoWindow()
        {
            InitializeComponent();
        }

        private void OnTitleInputEnd()
        {
            m_titleStr.Visibility = Visibility.Visible;
            m_titleInput.Visibility = Visibility.Collapsed;
            m_titleStr.Text = m_titleInput.Text;
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
            MinHeight = m_titleBar.ActualHeight + BorderThickness.Top + BorderThickness.Bottom;
            m_titleBarAnim = new ThicknessAnimation();
            m_titleBar.Margin = g_titleNormalMargin;
            //SYTEST
            OnTopMostClick(null, null);
            Closed += (sender1, e1) => Application.Current.Shutdown();
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
                if (Height == MinHeight)
                {
                    Height = m_unfoldWndHeight;
                }
                else
                {
                    m_unfoldWndHeight = Height;
                    Height = MinHeight;
                }
            }
        }

        private void TitleBar_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed)
            {
                return;
            }

            ResizeMode = ResizeMode.NoResize;
            DragMove();
            if (e.LeftButton == MouseButtonState.Released)
            {
                if (ResizeMode == ResizeMode.NoResize)
                {
                    ResizeMode = ResizeMode.CanResize;
                }
            }
        }

        private void TitleStr_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            m_titleStr.Visibility = Visibility.Collapsed;
            m_titleInput.Visibility = Visibility.Visible;
            m_titleInput.Text = m_titleStr.Text;
            m_titleInput.Focus();
        }

        private void TitleBar_MouseEnter(object sender, MouseEventArgs e)
        {
            m_titleBar.Cursor = Cursors.SizeAll;
        }

        private void TitleInput_LostFocus(object sender, RoutedEventArgs e)
        {
            OnTitleInputEnd();
        }

        private void TitleInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                OnTitleInputEnd();
            }
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            m_titleBarAnim.From = m_titleBar.Margin;
            m_titleBarAnim.To = new Thickness();
            m_titleBarAnim.Duration = new Duration(TimeSpan.Parse("0:0:0.2"));
            m_titleBar.BeginAnimation(MarginProperty, m_titleBarAnim);
        }

        private void Window_Deactivated(object sender, EventArgs e)
        {
            m_titleBarAnim.From = m_titleBar.Margin;
            m_titleBarAnim.To = g_titleNormalMargin;
            m_titleBarAnim.Duration = new Duration(TimeSpan.Parse("0:0:0.2"));
            m_titleBar.BeginAnimation(MarginProperty, m_titleBarAnim);
        }
    }
}
