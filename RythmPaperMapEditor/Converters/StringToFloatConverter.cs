using System;
using System.Globalization;
using System.Windows.Data;

namespace RythmPaperMapEditor.Converters
{
    public class StringToFloatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return float.TryParse((string)value, NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat,
                out var ret)
                ? ret
                : 0;
        }
    }
}