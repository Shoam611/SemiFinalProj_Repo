using System.Linq;
using tWpfMashUp_v0._0._1.Core;
using tWpfMashUp_v0._0._1.Sevices;
using System.Collections.ObjectModel;
using tWpfMashUp_v0._0._1.MVVM.Models;
using System.Windows;

namespace tWpfMashUp_v0._0._1.MVVM.ViewModels
{
    public class ChatAppViewModel : ObservableObject
    {
        //fields
        StoreService storeService;
        ChatsService chatsService;

        //full props
        UserModel loggedUser;
        public UserModel LoggedUser { get => loggedUser; set { loggedUser = value; onProppertyChange(); } }
        Chat selectedChat;
        public Chat SelectedChat { get => selectedChat; set { selectedChat = value; onProppertyChange(); UpdateChatInStore(); } }
        private string displayedUser;
        public string DisplayedUser { get { return displayedUser; } set { displayedUser = value;onProppertyChange();} }

        string message;
        public string Message { get => message; set { message = value; onProppertyChange(); } }
        //props
        public RelayCommand GoToGameCommand { get; set; }
        public RelayCommand FetchUserCommand { get; set; }
        public RelayCommand GetRandomChatCommand { get; set; }

        public ObservableCollection<Chat> OnlineContacts { get; set; }
        public ObservableCollection<Chat> OfflineContacts { get; set; }

        public ChatAppViewModel(StoreService storeService, ChatsService chatsService)
        {
            this.chatsService = chatsService;
            this.storeService = storeService;
            OfflineContacts = new ObservableCollection<Chat>();
            OnlineContacts = new ObservableCollection<Chat>();
            FetchUserCommand = new RelayCommand(o => FetchUserHandler());
            GetRandomChatCommand = new RelayCommand(o => GetRandomChat());
        }

        private async void GetRandomChat()
        {
            var isParsed = int.TryParse(Message, out int res);
            if (!isParsed) MessageBox.Show("NaN");
            var rndChat = await chatsService.GetChatAsync(res);
            if (rndChat != null && !(OfflineContacts.Where(c => c.Id == rndChat.Id).ToList().Count > 0))
                OnlineContacts.Add(rndChat);
        }

        private void FetchUserHandler()
        {
            LoggedUser = storeService.Get(CommonKeys.LoggedUser.ToString());
            DisplayedUser = $"{LoggedUser.UserName} (#{LoggedUser.Id})";
        }

        private void UpdateChatInStore() => storeService.Add(CommonKeys.CurrentChat.ToString(), SelectedChat);
    }
}
