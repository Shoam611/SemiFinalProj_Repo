using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public ObservableCollection<Chat> OnlineContacts  { get; set; }
        public ObservableCollection<Chat> OfflineContacts { get; set; }        
        public string SelectedContact { get; set; }
        public ChatAppViewModel(StoreService storeService)
        {
            this.storeService = storeService;
            FetchUserCommand = new RelayCommand(o => FetchUserHandler());
            OfflineContacts = new ObservableCollection<Chat>();
            OfflineContacts.Add(new Chat());
            OfflineContacts.Add(new Chat());
            OfflineContacts.Add(new Chat());
        }

        private void FetchUserHandler()
        {
            LoggedUser = storeService.Get(CommonKeys.LoggedUser.ToString());
          //  OfflineContacts = (ObservableCollection<Chat>)LoggedUser.ChatsA??new ObservableCollection<Chat>(new List<Chat> {new Chat(),new Chat() });
            
        }
    }
}
