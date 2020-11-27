using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Memo.tools;

namespace Memo
{
    /// <summary>
    /// OptionControl.xaml 的交互逻辑
    /// </summary>
    public partial class OptionControl : UserControl
    {
        private EventHandler m_hideHandler;
        private MemoWindow m_memoWnd;

        private Dictionary<uint, ColorRadioButton> m_radioBtnMap
            = new Dictionary<uint, ColorRadioButton>();

        public OptionControl()
        {
            InitializeComponent();
            InitColorCtrl();
            NormalHeight = Height;
            Height = 0;
        }

        public double NormalHeight { get; }

        public void Show(MemoWindow wnd, EventHandler hideHandler)
        {
            if (IsVisible)
            {
                return;
            }

            Visibility = Visibility.Visible;
            PlayAnim(false);
            m_memoWnd = wnd;
            InitChecked();
            m_hideHandler = hideHandler + delegate
            {
                Visibility = Visibility.Hidden;
                Console.WriteLine("~~~~~~~~~~~~~~"); //SYTest
            };
        }

        public void Hide()
        {
            if (!IsVisible)
            {
                return;
            }

            PlayAnim(true, m_hideHandler);
        }

        private void InitChecked()
        {
            if (!m_radioBtnMap.TryGetValue(m_memoWnd.ThemeId, out ColorRadioButton btn))
            {
                return;
            }

            btn.IsChecked = true;
        }

        private void InitColorCtrl()
        {
            m_colorGrid.Children.Clear();
            foreach (ThemeInfo info in Global.ColorInfos)
            {
                var btn = new ColorRadioButton
                {
                    Background = new SolidColorBrush(Tools.ColorFromString(info.DisplayColor)),
                    ToolTip = info.Desc,
                    DataContext = info.Id,
                    MarkStroke = info.IsDark ? Brushes.White : Brushes.Black
                };

                m_radioBtnMap.Add(info.Id, btn);
                btn.Click += OnColorClick;
                m_colorGrid.Children.Add(btn);
            }
        }

        private void PlayAnim(bool isReverse, EventHandler onFinished = null)
        {
            var doubleAnim = new DoubleAnimation
            {
                From = Height,
                To = isReverse ? 0 : NormalHeight,
                Duration = new Duration(TimeSpan.Parse("0:0:0.2"))
            };
            
            if (null != onFinished)
            {
                doubleAnim.Completed += onFinished;
            }

            BeginAnimation(HeightProperty, doubleAnim);
        }

        private void OnColorClick(object sender, RoutedEventArgs e)
        {
            if (!(sender is RadioButton btn))
            {
                return;
            }

            var themeId = (uint) btn.DataContext;
            if (m_memoWnd.ThemeId != themeId)
            {
                m_memoWnd.SetTheme(themeId);
                Global.CurrentThemeId = themeId;
            }

            Hide();
        }
    }
}