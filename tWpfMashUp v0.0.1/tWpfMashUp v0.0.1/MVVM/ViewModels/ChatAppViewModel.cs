using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
        Chat selectedChat;
        public Chat SelectedChat{ get => selectedChat; set { selectedChat = value; onProppertyChange();UpdatChatIneStore(); } }


        public string ToUser { get; set; }
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
            OfflineContacts = new ObservableCollection<Chat>();
            OnlineContacts = new ObservableCollection<Chat>();

            FetchUserCommand = new RelayCommand(o => FetchUserHandler());
            GetRandomChatCommand = new RelayCommand(o => GetRandomChat());
        }

        private void UpdatChatIneStore() => storeService.Add(CommonKeys.CurrentChat.ToString(), SelectedChat);

        private async void GetRandomChat()
        {
            var rndChat=await chatsService.GetRandomChatAsync();
            if(rndChat!=null && ! (OfflineContacts.Where(c=>c.Id==rndChat.Id).ToList().Count > 0))
            OnlineContacts.Add(rndChat);
        }
        private async void GetChat()
        {
            var rndChat = await chatsService.GetRandomChatAsync(int.Parse(ToUser));
            if (rndChat != null && !(OfflineContacts.Where(c => c.Id == rndChat.Id).ToList().Count > 0))
                OfflineContacts.Add(rndChat);
        }
        private void FetchUserHandler()
        {
            LoggedUser = storeService.Get(CommonKeys.LoggedUser.ToString());
        }
    }
}
