using Memo.tools;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Memo
{
    /// <summary>
    /// OptionWindow.xaml 的交互逻辑
    /// </summary>
    public partial class OptionWindow : Window
    {
        private MemoWindow m_memoWnd;
        private Dictionary<uint, ColorRadioButton> m_radioBtnMap
            = new Dictionary<uint, ColorRadioButton>();


        public OptionWindow()
        {
            InitializeComponent();
            InitColorPanel();
        }

        private void InitChecked()
        {
            if (!m_radioBtnMap.TryGetValue(m_memoWnd.ThemeId, out ColorRadioButton btn))
            {
                return;
            }

            btn.IsChecked = true;
        }

        private void InitColorPanel()
        {
            m_colorPanel.Children.Clear();
            double itemWidth = m_colorPanel.Width / Global.ColorInfos.Count;
            foreach (var info in Global.ColorInfos)
            {
                ColorRadioButton btn = new ColorRadioButton
                {
                    Style = TryFindResource("RadioThemeColor") as Style,
                    Background = new SolidColorBrush(Tools.ColorFromString(info.DisplayColor)),
                    ToolTip = info.Desc,
                    Width = itemWidth,
                    DataContext = info,
                    MarkStroke = info.IsDark ? Brushes.White : Brushes.Black,
                };

                m_radioBtnMap.Add(info.Id, btn);
                btn.Click += OnColorClick;
                m_colorPanel.Children.Add(btn);
            }
        }

        private void OnColorClick(object sender, RoutedEventArgs e)
        {
            if (!(sender is RadioButton btn))
            {
                return;
            }

            ThemeInfo info = btn.DataContext as ThemeInfo;
            if (m_memoWnd.ThemeId != info.Id)
            {
                m_memoWnd.SetTheme(info);
            }

            Hide();
        }

        private void Window_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            bool isShow = (bool)e.NewValue;
            if (isShow)
            {
                m_memoWnd = Owner as MemoWindow;
                InitChecked();
            }
            else
            {
                m_memoWnd = null;
                Owner = null;
            }
        }
    }
}
