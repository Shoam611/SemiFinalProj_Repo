using System.Windows;
using System.Windows.Controls;
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
        public string UserName { get { return userName; } set { userName = value; onProppertyChange(); } }

        private string password;
        public string Password { get { return password; } set { password = value; onProppertyChange(); } }

        public LoginViewModel(AuthenticationService authService)
        {
            this.authService = authService;
            UserName = "Username";
            SighUpCommand = new RelayCommand((o) => SighnUpHandler());
            AuthenticateCommand = new RelayCommand(o => LogInHandler());
            PasswordChangedCommand = new RelayCommand((o) => HandlePasswordChanged(o as RoutedEventArgs));
        }

        private async void SighnUpHandler()
        {
            var isSighnedUp = await authService.CallServerToSignUp(UserName, Password);
            if (isSighnedUp) LogInHandler();
            else MessageBox.Show("Unexpected Error while sighning up");
        }
        private async void LogInHandler()
        {
            var isAuthenticated = await authService.LoginAsync(UserName, Password);
            if (!isAuthenticated)
            {
                MessageBox.Show("User not found");
            }
        }

        void HandlePasswordChanged(RoutedEventArgs args) => Password = (args.Source as PasswordBox).Password;
    }
}