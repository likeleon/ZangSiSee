using System;
using System.Globalization;
using Xamarin.Forms;

namespace ZangSiSee.Converters
{
    public class RoundedIntToDoubleConverter : IValueConverter
    {
        public static RoundedIntToDoubleConverter Instance { get; } = new RoundedIntToDoubleConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return System.Convert.ToDouble(value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || !(value is double))
                return 0;
            
            return (int)Math.Round((double)value);
        }
    }
}
