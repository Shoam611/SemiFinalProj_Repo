using System;
using tWpfMashUp_v0._0._1.Core;
using tWpfMashUp_v0._0._1.MVVM.Views;
using tWpfMashUp_v0._0._1.Sevices;

namespace tWpfMashUp_v0._0._1.MVVM.ViewModels
{
   public class MainViewModel : ObservableObject
    {
        private object view;
        private readonly AuthenticationService authenticationService;
        private readonly SignalRListinerService signalRListener;

        public object View
        {
            get => view;
            set { view = value; onProppertyChange(); }
        }

        public MainViewModel(SignalRListenerService signalRListiner,AuthenticationService authenticationService)
        {
            this.authenticationService = authenticationService;
            this.signalRListener = signalRListiner;
            View = new LoginView();
            authenticationService.LoggingIn +=(s,e)=> SetViewTransition("Chat");
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
