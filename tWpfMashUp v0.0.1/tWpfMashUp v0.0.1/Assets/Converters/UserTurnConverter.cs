using Microsoft.Extensions.DependencyInjection;
using System;
using System.Globalization;
using System.Windows;
using tWpfMashUp_v0._0._1.MVVM.Models;
using tWpfMashUp_v0._0._1.Sevices;

namespace tWpfMashUp_v0._0._1.Assets.Converters
{
    public class UserTurnConverter : BaseConverter<UserTurnConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var store = App.ServiceProvider.GetRequiredService<StoreService>();
            var user = (User)store.Get(CommonKeys.WithUser.ToString()).UserName;

            if ((bool)value)
                return "Your turn";

            return $"{user.UserName}";
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
