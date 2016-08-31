using System;
using System.Globalization;
using System.IO;
using Xamarin.Forms;

namespace ZangSiSee.Converters
{
    public class ByteArrayToImageSourceConverter : IValueConverter
    {
        public static ByteArrayToImageSourceConverter Instance { get; } = new ByteArrayToImageSourceConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var bytes = (byte[])value;
            if (bytes == null)
                return null;
            return ImageSource.FromStream(() => new MemoryStream(bytes));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
