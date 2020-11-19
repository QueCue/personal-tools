using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SYControlLibrary
{
    public class NineGridBorder : Border
    {

        public ImageSource Image
        {
            get { return (ImageSource)base.GetValue(ImageProperty); }
            set { base.SetValue(ImageProperty, value); }
        }


        public static readonly DependencyProperty ImageProperty = DependencyProperty.Register("Image",
            typeof(ImageSource), typeof(NineGridBorder),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));


        public Thickness ImageMargin
        {
            get { return (Thickness)GetValue(ImageMarginProperty); }
            set { SetValue(ImageMarginProperty, value); }
        }


        public static readonly DependencyProperty ImageMarginProperty =
            DependencyProperty.Register("ImageMargin", typeof(Thickness), typeof(NineGridBorder),
                new FrameworkPropertyMetadata(new Thickness(), FrameworkPropertyMetadataOptions.AffectsRender));




        public double ImageOpacity
        {
            get { return (double)GetValue(ImageOpacityProperty); }
            set { SetValue(ImageOpacityProperty, value); }
        }

        public static readonly DependencyProperty ImageOpacityProperty =
            DependencyProperty.Register("ImageOpacity", typeof(double), typeof(NineGridBorder),
                new FrameworkPropertyMetadata(1D, FrameworkPropertyMetadataOptions.AffectsRender));



        private bool IsNineGrid
        {
            get { return !ImageMargin.Equals(new Thickness()); }
        }




        protected override void OnRender(DrawingContext dc)
        {
            DrawImage(dc, new Rect(0, 0, RenderSize.Width, RenderSize.Height));
        }

        private void DrawImage(DrawingContext dc, Rect rect)
        {
            ImageSource source = Image;

            if (source != null)
            {
                double opacity = ImageOpacity;

                if (IsNineGrid)
                {

                    Thickness margin = Clamp(ImageMargin, new Size(source.Width, source.Height), rect.Size);

                    double[] xGuidelines = { 0, margin.Left, rect.Width - margin.Right, rect.Width };
                    double[] yGuidelines = { 0, margin.Top, rect.Height - margin.Bottom, rect.Height };
                    GuidelineSet guidelineSet = new GuidelineSet(xGuidelines, yGuidelines);
                    guidelineSet.Freeze();

                    dc.PushGuidelineSet(guidelineSet);

                    double[] vx = { 0D, margin.Left / source.Width, (source.Width - margin.Right) / source.Width, 1D };
                    double[] vy = { 0D, margin.Top / source.Height, (source.Height - margin.Bottom) / source.Height, 1D };
                    double[] x = { rect.Left, rect.Left + margin.Left, rect.Right - margin.Right, rect.Right };
                    double[] y = { rect.Top, rect.Top + margin.Top, rect.Bottom - margin.Bottom, rect.Bottom };

                    for (int i = 0; i < 3; ++i)
                    {
                        for (int j = 0; j < 3; ++j)
                        {
                            ImageBrush brush = new ImageBrush(source);
                            brush.Opacity = opacity;
                            brush.Viewbox = new Rect(vx[j], vy[i], Math.Max(0D, (vx[j + 1] - vx[j])),
                                Math.Max(0D, (vy[i + 1] - vy[i])));

                            dc.DrawRectangle(brush, null,
                                new Rect(x[j], y[i], Math.Max(0D, (x[j + 1] - x[j])), Math.Max(0D, (y[i + 1] - y[i]))));
                        }
                    }

                    dc.Pop();
                }
                else
                {
                    ImageBrush brush = new ImageBrush(source);
                    brush.Opacity = opacity;

                    dc.DrawRectangle(brush, null, rect);
                }
            }
        }

        private static Thickness Clamp(Thickness margin, Size firstMax, Size secondMax)
        {
            double left = Clamp(margin.Left, firstMax.Width, secondMax.Width);
            double top = Clamp(margin.Top, firstMax.Height, secondMax.Height);
            double right = Clamp(margin.Right, firstMax.Width - left, secondMax.Width - left);
            double bottom = Clamp(margin.Bottom, firstMax.Height - top, secondMax.Height - top);

            return new Thickness(left, top, right, bottom);
        }

        private static double Clamp(double value, double firstMax, double secondMax)
        {
            return Math.Max(0, Math.Min(Math.Min(value, firstMax), secondMax));
        }
    }
}
