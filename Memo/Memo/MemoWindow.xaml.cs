using Memo.tools;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace Memo
{
    /// <summary>
    /// MemoWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MemoWindow : Window
    {
        public uint ThemeId { get; private set; }

        private Window m_oriParentWnd;
        private double m_unfoldWndHeight;
        private ThicknessAnimation m_thicknessAnim;
        private Button[] m_btnsNeedChangeColor;

        private static readonly Thickness g_titleNormalMargin = new Thickness(0, 0, 0, 32);
        private static Dictionary<string, ImageSource> g_btnToImgBlackMap
            = new Dictionary<string, ImageSource>();
        private static Dictionary<string, ImageSource> g_btnToImgWhiteMap
            = new Dictionary<string, ImageSource>();

        public MemoWindow()
        {
            DataContext = this;
            InitializeComponent();
        }

        public void SetTheme(ThemeInfo info)
        {
            if (null == info)
            {
                return;
            }

            ThemeInfo preInfo = Global.GetThemeInfo(ThemeId);
            ThemeId = info.Id;
            m_titleBar.Background = new SolidColorBrush(Tools.ColorFromString(info.TitleBarColor));
            Background = new SolidColorBrush(Tools.ColorFromString(info.BgColor));
            if (info.IsDark == preInfo.IsDark)
            {
                return;
            }

            Foreground = new SolidColorBrush(info.IsDark ? Colors.White : Colors.Black);
            m_mainInput.Foreground = Foreground;
            foreach (var btn in m_btnsNeedChangeColor)
            {
                ChangeBtnColor(btn, info.IsDark);
            }
        }

        private unsafe void ChangeBtnColor(Button btn, bool isDark)
        {
            Dictionary<string, ImageSource> map = isDark ? g_btnToImgWhiteMap : g_btnToImgBlackMap;
            if (map.TryGetValue(btn.Name, out ImageSource source))
            {
                ((ImageBrush)btn.Background).ImageSource = source;
                ((ImageBrush)btn.Background).Opacity = isDark ? 1 : 0.6;
                return;
            }

            BitmapImage bi = new BitmapImage(new Uri(((ImageBrush)btn.Background).ImageSource.ToString()));
            var formatConvertedBitmap = new FormatConvertedBitmap();
            formatConvertedBitmap.BeginInit();
            formatConvertedBitmap.Source = bi;
            formatConvertedBitmap.DestinationFormat = PixelFormats.Bgra32;
            formatConvertedBitmap.EndInit();
            var writeableBitmap = new WriteableBitmap(formatConvertedBitmap);
            writeableBitmap.Lock();
            var length = writeableBitmap.PixelWidth
                * writeableBitmap.PixelHeight
                * writeableBitmap.Format.BitsPerPixel / 8;
            var backBuffer = (byte*)writeableBitmap.BackBuffer;
            if (null == backBuffer)
            {
                return;
            }

            for (int i = 0; i < length; i += 4)
            {
                var blue = backBuffer[i];
                var green = backBuffer[i + 1];
                var red = backBuffer[i + 2];
                if (0 == blue
                    && 0 == green
                    && 0 == red)
                {
                    blue = isDark ? (byte)255 : (byte)0;
                    green = isDark ? (byte)255 : (byte)0;
                    red = isDark ? (byte)255 : (byte)0;
                }

                backBuffer[i] = blue;
                backBuffer[i + 1] = green;
                backBuffer[i + 2] = red;
            }

            writeableBitmap.AddDirtyRect(new Int32Rect(0, 0, writeableBitmap.PixelWidth, writeableBitmap.PixelHeight));
            writeableBitmap.Unlock();
            map.Add(btn.Name, writeableBitmap);
            ((ImageBrush)btn.Background).ImageSource = writeableBitmap;
            ((ImageBrush)btn.Background).Opacity = isDark ? 1 : 0.6;
        }

        private void OnTitleInputEnd()
        {
            m_titleStr.Visibility = Visibility.Visible;
            m_titleInput.Visibility = Visibility.Collapsed;
            m_titleStr.Text = m_titleInput.Text;
            Name = m_titleStr.Text;
        }

        private void OnCloseClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void OnTopMostClick(object sender, RoutedEventArgs e)
        {
            Topmost = !Topmost;
            var tg = m_topmostBtn.RenderTransform as TransformGroup;
            var tgNew = tg?.CloneCurrentValue();
            if (null != tgNew)
            {
                m_topmostBtn.RenderTransformOrigin = new Point(0.5, 0.5);
                if (tgNew.Children[2] is RotateTransform rt)
                {
                    rt.Angle = Topmost ? 0 : 90;
                }
            }

            m_topmostBtn.RenderTransform = tgNew;
            Owner = Topmost ? null : m_oriParentWnd;
        }

        private void OnOptionClick(object sender, RoutedEventArgs e)
        {
            var wnd = Global.OptionWnd;
            wnd.Owner = this;
            wnd.Show();
        }

        private void MemoWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            m_oriParentWnd = Owner;
            ThemeId = Global.Default.Id;
            MinHeight = m_titleBar.ActualHeight + BorderThickness.Top + BorderThickness.Bottom;
            m_thicknessAnim = new ThicknessAnimation();
            m_titleBar.Margin = g_titleNormalMargin;
            m_btnsNeedChangeColor = new[] { m_newBtn, m_optionBtn, m_topmostBtn, m_closeBtn };
            if (null == g_btnToImgBlackMap
                || g_btnToImgBlackMap.Count <= 0)
            {
                foreach (var btn in m_btnsNeedChangeColor)
                {
                    g_btnToImgBlackMap?.Add(btn.Name, ((ImageBrush)btn.Background).ImageSource);
                }
            }

            //SYTEST
            OnTopMostClick(null, null);
            Closed += (sender1, e1) => Application.Current.Shutdown();
        }

        private void OnNewClick(object sender, RoutedEventArgs e)
        {
            ((MainWindow)m_oriParentWnd).NewMemo();
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

            if (e.LeftButton != MouseButtonState.Pressed
                || sender != e.Source)
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
            m_thicknessAnim.From = m_titleBar.Margin;
            m_thicknessAnim.To = new Thickness();
            m_thicknessAnim.Duration = new Duration(TimeSpan.Parse("0:0:0.2"));
            m_titleBar.BeginAnimation(MarginProperty, m_thicknessAnim);
        }

        private void Window_Deactivated(object sender, EventArgs e)
        {
            m_thicknessAnim.From = m_titleBar.Margin;
            m_thicknessAnim.To = g_titleNormalMargin;
            m_thicknessAnim.Duration = new Duration(TimeSpan.Parse("0:0:0.2"));
            m_titleBar.BeginAnimation(MarginProperty, m_thicknessAnim);
        }
    }
}
