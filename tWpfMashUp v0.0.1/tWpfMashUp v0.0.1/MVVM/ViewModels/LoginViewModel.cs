using System.Windows;
using System.Windows.Controls;
using tWpfMashUp_v0._0._1.Assets.Components.CustomModal;
using tWpfMashUp_v0._0._1.Core;
using tWpfMashUp_v0._0._1.Sevices;

namespace tWpfMashUp_v0._0._1.MVVM.ViewModels
{
    public class LoginViewModel : ObservableObject
    {
        private readonly AuthenticationService authService;
        public RelayCommand SighUpCommand { get; set; }
        public RelayCommand AuthenticateCommand { get; set; }
        public RelayCommand PasswordChangedCommand { get; set; }

        private string userName;
        public string UserName { get { return userName; } set { userName = value; OnProppertyChange(); } }

        private string password;
        public string Password { get { return password; } set { password = value; OnProppertyChange(); } }

        public LoginViewModel(AuthenticationService authService)
        {
            this.authService = authService;
            UserName = "User";
            SighUpCommand = new RelayCommand((o) => SighnUpHandler());
            AuthenticateCommand = new RelayCommand(o => LogInHandler());
            PasswordChangedCommand = new RelayCommand((o) => HandlePasswordChanged(o as RoutedEventArgs));
        }

        private async void SighnUpHandler()
        {
            var isSighnedUp = await authService.CallServerToSignUp(UserName, Password);
            if (isSighnedUp)
                LogInHandler();
        }
        private async void LogInHandler()
        {
            var isAuthenticated = await authService.LoginAsync(UserName, Password);
            if (!isAuthenticated)
            {
                Modal.ShowModal("Failed To Login!");
            }
        }

        private void HandlePasswordChanged(RoutedEventArgs args) => Password = (args.Source as PasswordBox).Password;
    }
}