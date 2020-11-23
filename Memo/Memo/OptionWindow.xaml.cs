using Memo.tools;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Memo
{
    /// <summary>
    /// OptionWindow.xaml 的交互逻辑
    /// </summary>
    public partial class OptionWindow : Window
    {
        public OptionWindow()
        {
            InitializeComponent();
        }

        private void InitColorPanel()
        {
            m_colorPanel.Children.Clear();
            double itemWidth = m_colorPanel.Width / Theme.ColorInfos.Count;
            foreach (var info in Theme.ColorInfos)
            {
                //string path = "pack://application:,,,/Memo;component/res/selected.png";
                //Uri uri = new Uri(path);
                //ImageSource source = new BitmapImage(uri);
                //Image img = new Image
                //{
                //    Source = source,
                //    Margin = new Thickness(16),
                //    Visibility = Visibility.Hidden,
                //    Stretch = Stretch.None,
                //};



                //Button button = new Button
                //{
                //    Style = TryFindResource("MyButton") as Style,
                //    Background = new SolidColorBrush(Tools.ColorFromString(info.DisplayColor)),
                //    ToolTip = info.Desc,
                //    Width = itemWidth,
                //    Content = img,
                //    DataContext = info
                //};

                //button.Click += OnColorClick;
                //m_colorPanel.Children.Add(button);

                RadioButton btn = new RadioButton
                {
                    Style = TryFindResource("RadioThemeColor") as Style,
                    Background = new SolidColorBrush(Tools.ColorFromString(info.DisplayColor)),
                    ToolTip = info.Desc,
                    Width = itemWidth,
                    DataContext = info
                };

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
            MemoWindow memoWnd = Owner as MemoWindow;
            if (memoWnd.ThemeId.Equals(info.Guid))
            {
                return;
            }

            memoWnd.SetTheme(info);
            //(btn.Content as Image).Visibility = Visibility.Visible;
            //Close();
        }

        private void OnNewClick(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            InitColorPanel();
        }
    }
}
