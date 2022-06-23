using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using RythmPaperMapEditor.ViewModels;

namespace RythmPaperMapEditor.Converters
{
    public class PlaybackStateToIconConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var iconPaths = (Application.Current.Resources["PlayIcon"], Application.Current.Resources["PauseIcon"]);
            return ((PlaybackState)value) == PlaybackState.Playing ? iconPaths.Item2 : iconPaths.Item1;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}