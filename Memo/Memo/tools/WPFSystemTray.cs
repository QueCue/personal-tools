using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Memo.tools
{
    public class WPFSystemTray
    {
        /// <summary>
        /// 设置系统托盘
        /// </summary>
        /// <param name="pars">最小化参数</param>
        /// <param name="menuList"></param>
        /// <returns></returns>
        public static NotifyIcon SetSystemTray(SystemTrayParameter pars, List<SystemTrayMenu> menuList)
        {
            var notifyIcon = new NotifyIcon();
            notifyIcon.Visible = true;
            if (!string.IsNullOrWhiteSpace(pars.Icon))
            {
                notifyIcon.Icon = new Icon(pars.Icon); //程序图标
            }

            if (!string.IsNullOrWhiteSpace(pars.MinText))
            {
                notifyIcon.Text = pars.MinText; //最小化到托盘时，鼠标悬浮时显示的文字
            }

            if (!string.IsNullOrWhiteSpace(pars.TipText))
            {
                notifyIcon.BalloonTipText = pars.TipText; //设置系统托盘启动时显示的文本
                notifyIcon.ShowBalloonTip(pars.Time == 0 ? 100 : pars.Time); //显示时长
            }

            notifyIcon.MouseDoubleClick += pars.DbClick; //双击事件
            notifyIcon.ContextMenuStrip = GetMenuStrip(menuList);
            return notifyIcon;
        }

        /// <summary>
        /// 设置系统托盘的菜单属性
        /// </summary>
        /// <param name="menus"></param>
        /// <returns></returns>
        private static ContextMenuStrip GetMenuStrip(List<SystemTrayMenu> menus)
        {
            var menu = new ContextMenuStrip();
            var menuArray = new ToolStripItem[menus.Count];
            var i = 0;
            foreach (SystemTrayMenu item in menus)
            {
                var menuItem = new ToolStripMenuItem();
                menuItem.Text = item.Txt;
                menuItem.Click += item.Click;
                if (!string.IsNullOrWhiteSpace(item.Icon) && File.Exists(item.Icon))
                {
                    menuItem.Image = Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + item.Icon);
                }

                menuArray[i++] = menuItem;
            }

            menu.Items.AddRange(menuArray);
            return menu;
        }
    }

    /// <summary>
    /// 系统托盘参数
    /// </summary>
    public class SystemTrayParameter
    {
        public SystemTrayParameter(string icon, string minText, string tipText, int time, MouseEventHandler dbClick)
        {
            Icon = icon;
            MinText = minText;
            TipText = tipText;
            Time = time;
            this.DbClick = dbClick;
        }

        /// <summary>
        /// 托盘显示图标
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// 最小化悬浮时文本
        /// </summary>
        public string MinText { get; set; }

        /// <summary>
        /// 最小化启动时文本
        /// </summary>
        public string TipText { get; set; }

        /// <summary>
        /// 最小化启动时文本显示时长
        /// </summary>
        public int Time { get; set; }

        /// <summary>
        /// 最小化双击事件
        /// </summary>
        public MouseEventHandler DbClick { get; set; }
    }

    /// <summary>
    /// 右键菜单
    /// </summary>
    public class SystemTrayMenu
    {
        /// <summary>
        /// 菜单文本
        /// </summary>
        public string Txt { get; set; }

        /// <summary>
        /// 菜单图标
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// 菜单单击事件
        /// </summary>
        public EventHandler Click { get; set; }
    }
}