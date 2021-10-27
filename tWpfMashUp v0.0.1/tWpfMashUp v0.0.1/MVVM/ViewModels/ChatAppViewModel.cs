using System;
using System.Collections.Generic;
using tWpfMashUp_v0._0._1.Core;
using tWpfMashUp_v0._0._1.MVVM.Models;
using tWpfMashUp_v0._0._1.Sevices;

namespace tWpfMashUp_v0._0._1.MVVM.ViewModels
{
    public class ChatAppViewModel :ObservableObject
    {
        StoreService storeService;

        public RelayCommand GoToGameCommand { get; set; }
        public RelayCommand FetchUserCommand { get; set; }
        UserModel loggedUser;
        public UserModel LoggedUser { get => loggedUser; set { loggedUser = value; onProppertyChange(); } }
        public List<string> OnlineContacts  { get; set; }
        public List<string> OfflineContacts { get; set; }        
        public string SelectedContact { get; set; }
        public ChatAppViewModel(StoreService storeService)
        {
            this.storeService = storeService;
            FetchUserCommand = new RelayCommand(o => FetchUserHandler());
        }

        private void FetchUserHandler()
        {
            LoggedUser = storeService.Get(CommonKeys.LoggedUser.ToString());
        }
    }
}
