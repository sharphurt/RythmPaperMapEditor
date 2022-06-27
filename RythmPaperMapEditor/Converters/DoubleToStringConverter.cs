using System;
using System.Globalization;
using System.Windows.Data;

namespace RythmPaperMapEditor.Converters
{
    public class DoubleToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToString();
            /*
            return (double)value == 0 ? "" : value.ToString();
        */
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return double.TryParse((string)value, NumberStyles.Any,
                CultureInfo.InvariantCulture, out var ret)
                ? ret
                : 0;
        }
    }
}