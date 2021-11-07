using System;
using System.Globalization;
using System.Windows;

namespace tWpfMashUp_v0._0._1.Assets.Converters
{
    public class SentByMeBackgroundConverter : BaseConverter<SentByMeBackgroundConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? Application.Current.FindResource("WordVeryLightBlueBrush") : Application.Current.FindResource("ForegroundLightBrush");
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
