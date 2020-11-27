using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Memo
{
    public class ImageButton : Button
    {
        private Action m_onGotFocus;

        static ImageButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ImageButton)
                , new FrameworkPropertyMetadata(typeof(ImageButton)));
        }

        public ImageButton()
        {
            GotFocus += (sender, e) => m_onGotFocus?.Invoke();
        }

        public static readonly DependencyProperty ImageViewportProperty =
            DependencyProperty.Register("ImageViewport"
             , typeof(Rect)
             , typeof(ImageButton)
             , new PropertyMetadata(new Rect(0, 0, 1, 1)));

        public Rect ImageViewport
        {
            get => (Rect)GetValue(ImageViewportProperty);
            set => SetValue(ImageViewportProperty, value);
        }

        public static readonly DependencyProperty ImageViewboxProperty =
            DependencyProperty.Register("ImageViewbox"
            , typeof(Rect)
            , typeof(ImageButton)
            , new PropertyMetadata(new Rect(0, 0, 1, 1)));

        public Rect ImageViewbox
        {
            get => (Rect)GetValue(ImageViewboxProperty);
            set => SetValue(ImageViewboxProperty, value);
        }

        public static readonly DependencyProperty ImageOpacityProperty =
            DependencyProperty.Register("ImageOpacity"
          , typeof(double)
          , typeof(ImageButton)
          , new PropertyMetadata(0.6));

        public double ImageOpacity
        {
            get => (double)GetValue(ImageOpacityProperty);
            set => SetValue(ImageOpacityProperty, value);
        }

        public static readonly DependencyProperty FocusTargetProperty =
          DependencyProperty.Register("FocusTarget"
              , typeof(UIElement)
              , typeof(ImageButton)
              , new PropertyMetadata(OnFocusTargetPropertyChanged));

        public UIElement FocusTarget
        {
            get => (UIElement)GetValue(FocusTargetProperty);
            set => SetValue(FocusTargetProperty, value);
        }

        private static void OnFocusTargetPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (!(sender is ImageButton btn))
            {
                return;
            }

            if (null == btn.FocusTarget)
            {
                btn.m_onGotFocus = null;
                return;
            }

            btn.m_onGotFocus = () => btn.FocusTarget.Focus();
        }

        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register("Source", typeof(string), typeof(ImageButton), new PropertyMetadata(OnSourcePropertyChanged));

        public string Source
        {
            get { return (string)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        private static void OnSourcePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (!(sender is ImageButton btn)
                || string.IsNullOrEmpty(btn.Source))
            {
                return;
            }

            var path = string.Format("pack://application:,,,/Memo;component/{0}", @btn.Source);
            if (!Uri.TryCreate(path, UriKind.RelativeOrAbsolute, out Uri uri))
            {
                return;
            }

            BitmapSource bitmap = new BitmapImage(uri);
            ImageBrush imageBrush = new ImageBrush(bitmap)
            {
                Stretch = Stretch.None,
                Opacity = btn.ImageOpacity,
                Viewport = btn.ImageViewport,
                Viewbox = btn.ImageViewbox
            };

            btn.Background = imageBrush;
        }
    }
}
