using System;
using System.Globalization;
using Xamarin.Forms;

namespace ZangSiSee
{
    public class InverseBoolConverter : IValueConverter
    {
        public static InverseBoolConverter Instance { get; } = new InverseBoolConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !(bool)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !(bool)value;
        }
    }
}
