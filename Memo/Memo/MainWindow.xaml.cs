using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;

namespace Memo
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        const int GWL_HWNDPARENT = -8;

        [DllImport("user32.dll")]
        private static extern IntPtr GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", SetLastError = true)]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpWindowClass, string lpWindowName);

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindowEx(IntPtr parentHandle, IntPtr childAfter, string className, string windowTitle);

        [DllImport("user32.dll")]
        static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        private IntPtr m_oriParent;
        private IntPtr m_desktopParent;
        private IntPtr m_currentWindow;

        public MainWindow()
        {
            InitializeComponent();
            MouseLeftButtonDown += (sender, e) => DragMove();
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e) { }

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

            SetWindowLong(m_currentWindow, GWL_HWNDPARENT, Topmost ? m_oriParent : m_desktopParent);
        }

        private void OnOptionClick(object sender, RoutedEventArgs e)
        {
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            //SYTest
            m_currentWindow = new WindowInteropHelper(Application.Current.MainWindow).Handle;
            m_oriParent = GetWindowLong(m_currentWindow, GWL_HWNDPARENT);
            m_desktopParent = FindWindowEx
                (FindWindowEx
                (FindWindow
                ("Progman", "Program Manager")
                , IntPtr.Zero
                , "SHELLDLL_DefView"
                , "")
                , IntPtr.Zero
                , "SysListView32"
                , "FolderView");
            SetWindowLong(m_currentWindow, GWL_HWNDPARENT, m_desktopParent);
        }

        private void OnSettingClick(object sender, RoutedEventArgs e)
        {

        }

        private void OnNewClick(object sender, RoutedEventArgs e)
        {

        }
    }
}