using Memo.tools;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;

namespace Memo
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        #region 系统托盘
        NotifyIcon notifyIcon;

        //托盘右键菜单集合
        private List<SystemTrayMenu> GetList()
        {
            List<SystemTrayMenu> ls = new List<SystemTrayMenu>
            {
                new SystemTrayMenu() { Txt = "新建", Click = OnNewClick },
                new SystemTrayMenu() { Txt = "设置", Icon = "../../res/setting.png", Click = OnSettingClick },
                new SystemTrayMenu() { Txt = "关于", Click = OnAboutClick },
                new SystemTrayMenu() { Txt = "退出", Click = OnExitClick }
            };

            return ls;
        }

        #region 托盘右键菜单

        private void OnNewClick(object sender, EventArgs e)
        {
            New();
        }

        private void OnSettingClick(object sender, EventArgs e)
        {

        }

        private void OnAboutClick(object sender, EventArgs e)
        {

        }

        private void OnExitClick(object sender, EventArgs e)
        {
            Close();
            System.Windows.Application.Current.Shutdown();
        }

        #endregion

        #endregion

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //系统托盘
            SystemTrayParameter pars = new SystemTrayParameter("../../res/icon.ico", "QC便笺", "", 0, null);
            notifyIcon = WPFSystemTray.SetSystemTray(pars, GetList());
            var curWnd = new WindowInteropHelper(this).Handle;
            var desktopParent = Win32.FindWindowEx
                  (Win32.FindWindowEx
                  (Win32.FindWindow
                  ("Progman", "Program Manager")
                  , IntPtr.Zero
                  , "SHELLDLL_DefView"
                  , "")
                  , IntPtr.Zero
                  , "SysListView32"
                  , "FolderView");
            Win32.SetWindowLong(curWnd, Win32.GWL_HWNDPARENT, desktopParent);
            New();
            Hide();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            notifyIcon.Visible = false;
        }

        public void New()
        {
            var wnd = new MemoWindow { Owner = this };
            wnd.Show();
        }
    }
}