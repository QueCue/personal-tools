using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;
using Memo.tools;
using Application = System.Windows.Application;

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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //系统托盘
            var pars = new SystemTrayParameter("../../res/icon.ico", "QC便笺", "", 0, null);
            m_notifyIcon = WPFSystemTray.SetSystemTray(pars, GetList());
            //钉在桌面上
            IntPtr curWnd = new WindowInteropHelper(this).Handle;
            IntPtr desktopParent = Win32.FindWindowEx
            (Win32.FindWindowEx
                (Win32.FindWindow
                        ("Progman", "Program Manager")
                    , IntPtr.Zero
                    , "SHELLDLL_DefView"
                    , "")
                , IntPtr.Zero
                , "SysListView32"
                , "FolderView");
            Win32.SetWindowLong(curWnd, Win32.GwlHwndparent, desktopParent);
            NewMemo();
            Hide();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            m_notifyIcon.Visible = false;
        }

        public void NewMemo()
        {
            var wnd = new MemoWindow {Owner = this};
            wnd.Show();
        }

        #region 系统托盘

        private NotifyIcon m_notifyIcon;

        //托盘右键菜单集合
        private List<SystemTrayMenu> GetList()
        {
            var ls = new List<SystemTrayMenu>
            {
                new SystemTrayMenu {Txt = "新建", Click = OnNewClick},
                new SystemTrayMenu {Txt = "设置", Icon = "../../res/setting.png", Click = OnSettingClick},
                new SystemTrayMenu {Txt = "关于", Click = OnAboutClick},
                new SystemTrayMenu {Txt = "退出", Click = OnExitClick}
            };

            return ls;
        }

        #region 托盘右键菜单

        private void OnNewClick(object sender, EventArgs e)
        {
            NewMemo();
        }

        private void OnSettingClick(object sender, EventArgs e) { }

        private void OnAboutClick(object sender, EventArgs e) { }

        private void OnExitClick(object sender, EventArgs e)
        {
            Close();
            Application.Current.Shutdown();
        }

        #endregion

        #endregion
    }
}