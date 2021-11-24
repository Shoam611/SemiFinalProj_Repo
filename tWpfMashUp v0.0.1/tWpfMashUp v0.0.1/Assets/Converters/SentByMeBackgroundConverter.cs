using Microsoft.Extensions.DependencyInjection;
using System;
using System.Globalization;
using System.Windows;
using tWpfMashUp_v0._0._1.MVVM.Models;
using tWpfMashUp_v0._0._1.Sevices;

namespace tWpfMashUp_v0._0._1.Assets.Converters
{
    public class SentByMeBackgroundConverter : BaseConverter<SentByMeBackgroundConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var store = App.ServiceProvider.GetRequiredService<StoreService>();
            var user = store.Get(CommonKeys.LoggedUser.ToString()).UserName;

            if ((string)value == user)
            {
                return Application.Current.FindResource("BrightBrush");
            }

            else
            {
                return Application.Current.FindResource("PrimeryBrush");
            }
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}
