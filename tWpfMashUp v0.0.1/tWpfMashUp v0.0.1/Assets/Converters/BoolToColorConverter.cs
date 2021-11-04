using System;
using System.Windows.Data;
using System.Windows.Media;
using System.Globalization;

namespace tWpfMashUp_v0._0._1.Assets.Converters
{
    public class BoolToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var color = new SolidColorBrush(Colors.CornflowerBlue);
            if (value is bool b)
                if (b)
                    color = new SolidColorBrush(Colors.Green);
            return color;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
