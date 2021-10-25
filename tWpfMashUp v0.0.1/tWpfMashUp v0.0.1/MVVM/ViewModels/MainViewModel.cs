using System;
using tWpfMashUp_v0._0._1.Core;
using tWpfMashUp_v0._0._1.MVVM.Views;

namespace tWpfMashUp_v0._0._1.MVVM.ViewModels
{
   public class MainViewModel : ObservableObject
    {
        private object view;
        public object View
        {
            get => view;
            set { view = value; onProppertyChange(); }
        }

        //public RelayCommand OnLoadedCommand { get; set; }
        
        public MainViewModel() => View = new LoginView();


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
