using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SYControlLibrary
{
    /// <summary>
    /// SYButtonEX.xaml 的交互逻辑
    /// </summary>
    public partial class SYButtonEX : Button
    {
        static SYButtonEX()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SYButtonEX), new FrameworkPropertyMetadata(typeof(SYButtonEX)));
        }

        public static readonly DependencyProperty AutoSizeProperty =
            DependencyProperty.Register("AutoSize", typeof(bool), typeof(SYButtonEX), new PropertyMetadata(true));
        /// <summary>
        /// 是否自动调整大小
        /// </summary>
        public bool AutoSize
        {
            get { return (bool)GetValue(AutoSizeProperty); }
            set { SetValue(AutoSizeProperty, value); }
        }

        public static readonly DependencyProperty ImageSourceProperty =
            DependencyProperty.Register("ImageSource", typeof(string), typeof(SYButtonEX), new PropertyMetadata(OnSourcePropertyChanged));
        /// <summary>
        /// 鼠标进入背景样式
        /// </summary>
        public string ImageSource
        {
            get { return (string)GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
        }

        private static void OnSourcePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            SYButtonEX btn = sender as SYButtonEX;
            if (null == btn
                || string.IsNullOrEmpty(btn.ImageSource))
            {
                return;
            }

            var path = string.Format("pack://application:,,,/Memo;component/{0}", @btn.ImageSource);
            Uri uri;
            if (!Uri.TryCreate(path, UriKind.RelativeOrAbsolute, out uri))
            {
                return;
            }

            try
            {
                BitmapSource bitmap = new BitmapImage(uri);
                int width = (int)bitmap.PixelWidth;
                int height = (int)(bitmap.PixelHeight / 4);
                ImageBrush[] ibs = new ImageBrush[4];
                for (int i = 0; i < ibs.Length; i++)
                {
                    Int32Rect cut = new Int32Rect(0, height * i, width, height);
                    var cb = new CroppedBitmap(bitmap, cut);
                    ibs[i] = new ImageBrush(cb);
                }

                btn.Background = ibs[0];
                btn.MouseOverBackground = ibs[1];
                btn.PressedBackground = ibs[2];
                btn.DisabledBackground = ibs[3];
                if (btn.AutoSize)
                {
                    btn.Width = width;
                    btn.Height = height;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        private static void OnSourcePropertyChanged2(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            SYButtonEX btn = sender as SYButtonEX;
            if (null == btn
                || string.IsNullOrEmpty(btn.ImageSource))
            {
                return;
            }

            var path = string.Format("pack://application:,,,/Memo;component/{0}", @btn.ImageSource);
            Uri uri;
            try
            {
                uri = new Uri(path + "_Normal.png");
                btn.Background = new ImageBrush(new BitmapImage(uri));
                uri = new Uri(path + "_Hover.png");
                btn.MouseOverBackground = new ImageBrush(new BitmapImage(uri));
                uri = new Uri(path + "_Pressed.png");
                btn.PressedBackground = new ImageBrush(new BitmapImage(uri));
                uri = new Uri(path + "_Disabled.png");
                btn.DisabledBackground = new ImageBrush(new BitmapImage(uri));
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        //public static readonly DependencyProperty MainImageProperty =
        //    DependencyProperty.Register("MainImage", typeof(Image), typeof(SYButton), new PropertyMetadata(new Image()));
        ///// <summary>
        ///// 鼠标进入背景样式
        ///// </summary>
        //public Image MainImage
        //{
        //    get { return (Image)GetValue(MainImageProperty); }
        //    set { SetValue(MainImageProperty, value); }
        //}

        public static new readonly DependencyProperty BackgroundProperty =
            DependencyProperty.Register("Background", typeof(ImageBrush), typeof(SYButtonEX), new PropertyMetadata(new ImageBrush()));
        /// <summary>
        /// 鼠标正常背景样式
        /// </summary>
        [TypeConverter(typeof(StringToBrushTypeConverter))]
        public new ImageBrush Background
        {
            get { return (ImageBrush)GetValue(BackgroundProperty); }
            set { SetValue(BackgroundProperty, value); }
        }

        public static readonly DependencyProperty MouseOverBackgroundProperty =
            DependencyProperty.Register("MouseOverBackground", typeof(ImageBrush), typeof(SYButtonEX), new PropertyMetadata(new ImageBrush()));
        /// <summary>
        /// 鼠标进入背景样式
        /// </summary>
        [TypeConverter(typeof(StringToBrushTypeConverter))]
        public ImageBrush MouseOverBackground
        {
            get { return (ImageBrush)GetValue(MouseOverBackgroundProperty); }
            set { SetValue(MouseOverBackgroundProperty, value); }
        }

        public static readonly DependencyProperty MouseOverForegroundProperty =
            DependencyProperty.Register("MouseOverForeground", typeof(Brush), typeof(SYButtonEX), new PropertyMetadata(Brushes.White));
        /// <summary>
        /// 鼠标进入前景样式
        /// </summary>
        public Brush MouseOverForeground
        {
            get { return (Brush)GetValue(MouseOverForegroundProperty); }
            set { SetValue(MouseOverForegroundProperty, value); }
        }

        public static readonly DependencyProperty PressedBackgroundProperty =
            DependencyProperty.Register("PressedBackground", typeof(ImageBrush), typeof(SYButtonEX), new PropertyMetadata(new ImageBrush()));
        /// <summary>
        /// 鼠标按下背景样式
        /// </summary>
        [TypeConverter(typeof(StringToBrushTypeConverter))]
        public ImageBrush PressedBackground
        {
            get { return (ImageBrush)GetValue(PressedBackgroundProperty); }
            set { SetValue(PressedBackgroundProperty, value); }
        }

        public static readonly DependencyProperty PressedForegroundProperty =
            DependencyProperty.Register("PressedForeground", typeof(Brush), typeof(SYButtonEX), new PropertyMetadata(Brushes.White));
        /// <summary>
        /// 鼠标按下前景样式（图标、文字）
        /// </summary>
        public Brush PressedForeground
        {
            get { return (Brush)GetValue(PressedForegroundProperty); }
            set { SetValue(PressedForegroundProperty, value); }
        }

        public static readonly DependencyProperty DisabledBackgroundProperty =
            DependencyProperty.Register("DisabledBackground", typeof(ImageBrush), typeof(SYButtonEX), new PropertyMetadata(new ImageBrush()));
        /// <summary>
        /// 鼠标进入背景样式
        /// </summary>
        [TypeConverter(typeof(StringToBrushTypeConverter))]
        public ImageBrush DisabledBackground
        {
            get { return (ImageBrush)GetValue(DisabledBackgroundProperty); }
            set { SetValue(DisabledBackgroundProperty, value); }
        }

        public static readonly DependencyProperty DisabledForegroundProperty =
            DependencyProperty.Register("DisabledForeground", typeof(Brush), typeof(SYButtonEX), new PropertyMetadata(Brushes.White));
        /// <summary>
        /// 鼠标进入前景样式
        /// </summary>
        public Brush DisabledForeground
        {
            get { return (Brush)GetValue(DisabledForegroundProperty); }
            set { SetValue(DisabledForegroundProperty, value); }
        }

        public static readonly DependencyProperty ContentDecorationsProperty = DependencyProperty.Register(
            "ContentDecorations", typeof(TextDecorationCollection), typeof(SYButtonEX), new PropertyMetadata(null));
        public TextDecorationCollection ContentDecorations
        {
            get { return (TextDecorationCollection)GetValue(ContentDecorationsProperty); }
            set { SetValue(ContentDecorationsProperty, value); }
        }
    }
}
