using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SYControlLibrary
{
    public class StringToBrushTypeConverter : TypeConverter
    {
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (!(value is string))
            {
                return base.ConvertFrom(context, culture, value);
            }

            string str = value.ToString();
            var path = string.Format("pack://application:,,,/{0}", str);
            Uri uri;
            if (!Uri.TryCreate(path, UriKind.RelativeOrAbsolute, out uri))
            {
                return base.ConvertFrom(context, culture, value);
            }

            return new ImageBrush(new BitmapImage(uri));
        }
    }

    public class StringToImageBrushValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is string))
            {
                return null;
            }

            string str = value.ToString();
            //var path = string.Format("pack://application:,,,/{0}", str);
            var path = str;
            Uri uri;
            if (!Uri.TryCreate(path, UriKind.RelativeOrAbsolute, out uri))
            {
                return null;
            }

            return new ImageBrush(new BitmapImage(uri));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
