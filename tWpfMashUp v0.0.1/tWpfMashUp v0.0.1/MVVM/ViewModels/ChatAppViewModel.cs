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
        ChatsService chatsService;
        UserModel loggedUser;
        public UserModel LoggedUser { get => loggedUser; set { loggedUser = value; onProppertyChange(); } }

        public RelayCommand GoToGameCommand { get; set; }
        public RelayCommand FetchUserCommand { get; set; }
        public RelayCommand GetRandomChatCommand { get; set; }
        
        public ObservableCollection<Chat> OnlineContacts  { get; set; }
        public ObservableCollection<Chat> OfflineContacts { get; set; }        
        public string SelectedContact { get; set; }
        
        public ChatAppViewModel(StoreService storeService,ChatsService chatsService)
        {
            this.chatsService = chatsService;
            this.storeService = storeService;
            FetchUserCommand = new RelayCommand(o => FetchUserHandler());
            GetRandomChatCommand = new RelayCommand(o => GetRandomChat());
        }

        private void GetRandomChat()
        {
            OnlineContacts.Add(chatsService.GetRandomChatAsync());
        }

        private void FetchUserHandler()
        {
            LoggedUser = storeService.Get(CommonKeys.LoggedUser.ToString());
        }
    }
}
