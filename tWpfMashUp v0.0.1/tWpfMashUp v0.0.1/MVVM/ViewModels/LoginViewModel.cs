using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using tWpfMashUp_v0._0._1.Core;

namespace tWpfMashUp_v0._0._1.MVVM.ViewModels
{
    public class LoginViewModel :ObservableObject
    {
        private string userName;
        public string UserName
        {
            get { return userName; }
            set { userName = value; onProppertyChange(); }
        }
        private string password;

        public string Password
        {
            get { return password; }
            set { password = value; onProppertyChange(); }
        }

        public RelayCommand LoginCommand  { get; set; }
        public RelayCommand SighUpCommand { get; set; }
        public RelayCommand PasswordChangedCommand { get; set; }
        public LoginViewModel()
        {
            UserName = "Usename";          
            LoginCommand = new RelayCommand((o) => { }); ;
            SighUpCommand = new RelayCommand((o) => SighnUpHandler());
            PasswordChangedCommand= new RelayCommand((o) => HandlePasswordChanged(o as RoutedEventArgs));
        }

        private void SighnUpHandler() => Debug.WriteLine($"UserName: {UserName} , Paasord : {Password}");

        private void HandlePasswordChanged(RoutedEventArgs args) => Password = (args.Source as PasswordBox).Password;
    }
}
