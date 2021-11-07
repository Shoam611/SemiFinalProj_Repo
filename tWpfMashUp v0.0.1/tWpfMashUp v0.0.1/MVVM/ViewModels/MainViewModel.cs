using System;
using System.Windows;
using System.Windows.Input;
using tWpfMashUp_v0._0._1.Core;
using tWpfMashUp_v0._0._1.MVVM.Views;
using tWpfMashUp_v0._0._1.Sevices;

namespace tWpfMashUp_v0._0._1.MVVM.ViewModels
{
    public class MainViewModel : ObservableObject
    {
        private readonly AuthenticationService authenticationService;
        private readonly SignalRListenerService signalRListener;
        public RelayCommand MinimizeCommand { get; set; }
        public RelayCommand MaximizeCommand { get; set; }
        public RelayCommand CloseCommand { get; set; }
        public RelayCommand MouseDownCommand { get; set; }

        private object view;
        public object View { get => view; set { view = value; onProppertyChange(); } }

        public MainViewModel(SignalRListenerService signalRListiner, AuthenticationService authenticationService)
        {
            MinimizeCommand = new RelayCommand(o => Application.Current.MainWindow.WindowState = WindowState.Minimized);
            MaximizeCommand = new RelayCommand(o => OnMaximizeCommand());
            CloseCommand = new RelayCommand(o => Application.Current.Shutdown());
            MouseDownCommand = new RelayCommand(o => OnMouseDown(o as MouseButtonEventArgs));
            this.authenticationService = authenticationService;
            this.signalRListener = signalRListiner;
            View = new LoginView();
            authenticationService.LoggingIn += (s, e) => SetViewTransition("Chat");
        }

        private void OnMouseDown(MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed) Application.Current.MainWindow.DragMove();
        }

        private void OnMaximizeCommand()
        {
            if (Application.Current.MainWindow.WindowState != WindowState.Maximized)
                Application.Current.MainWindow.WindowState = WindowState.Maximized;
            else
                Application.Current.MainWindow.WindowState = WindowState.Normal;
        }

        public void SetViewTransition(string option)
        {
            View = option switch
            {
                "Game" => new ChatAndGameView(),
                "Auth" => new LoginView(),
                "Chat" => new ChatAppView(),
                _ => new LoginView(),
            };
        }
    }
}
