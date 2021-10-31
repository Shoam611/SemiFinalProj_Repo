using System;
using tWpfMashUp_v0._0._1.Core;
using tWpfMashUp_v0._0._1.MVVM.Views;
using tWpfMashUp_v0._0._1.Sevices;

namespace tWpfMashUp_v0._0._1.MVVM.ViewModels
{
   public class MainViewModel : ObservableObject
    {
        private object view;
        private AuthenticationService authenticationService;
        private SignalRListinerService signalRListener;

        public object View
        {
            get => view;
            set { view = value; onProppertyChange(); }
        }

        public MainViewModel(SignalRListinerService signalRListiner,AuthenticationService authenticationService)
        {
            this.authenticationService = authenticationService;
            this.signalRListener = signalRListiner;
            View = new LoginView();
            authenticationService.LoggingIn +=(s,e)=> SetViewTransition("Chat");
        }

        public void SetViewTransition(string option)
        {
            switch (option)
            {
                case "Game":View = new ChatAndGameView();break;
                case "Auth":View = new LoginView();break;
                case "Chat":View = new ChatAppView();break;
                default: View = new LoginView(); break;
            }
        }
    }
}
