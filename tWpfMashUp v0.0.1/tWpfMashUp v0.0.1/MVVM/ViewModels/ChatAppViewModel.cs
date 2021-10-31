using System.Linq;
using tWpfMashUp_v0._0._1.Core;
using tWpfMashUp_v0._0._1.Sevices;
using System.Collections.ObjectModel;
using tWpfMashUp_v0._0._1.MVVM.Models;
using System.Windows;
using System.Windows.Controls;

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
        public string BindingTest { get => message; set { message = value; onProppertyChange(); } }
        //props
        public RelayCommand GoToGameCommand { get; set; }
        public RelayCommand FetchUserCommand { get; set; }
        public RelayCommand GetRandomChatCommand { get; set; }
        public RelayCommand OnSelectionChangedCommand { get; set; }

        public ObservableCollection<Chat> OnlineContacts { get; set; }
        public ObservableCollection<Chat> OfflineContacts { get; set; }

        public ChatAppViewModel(StoreService storeService, ChatsService chatsService)
        {
            this.chatsService = chatsService;
            this.storeService = storeService;
            OfflineContacts = new ObservableCollection<Chat>();
            OnlineContacts = new ObservableCollection<Chat>();
            FetchUserCommand = new RelayCommand(o => FetchUserHandler());
            GetRandomChatCommand = new RelayCommand(o => GetChat());
           // OnSelectionChangedCommand = new RelayCommand(o => HandleSelectionChanged(o as RoutedEventArgs));
        }

        public void HandleSelectionChanged(SelectionChangedEventArgs selectionChangedEventArgs)
        {
            var newCurrentChat = (Chat)selectionChangedEventArgs.AddedItems[0];
            storeService.Add(CommonKeys.CurrentChat.ToString(), newCurrentChat);
        }

        private async void GetChat()
        {
            var isParsed = int.TryParse(BindingTest, out int res);
            if (!isParsed) { MessageBox.Show("NaN");return; }
            var newChat = await chatsService.GetChatAsync(res);
            if (newChat != null && !(OnlineContacts.Where(c => c.Id == newChat.Id).ToList().Count > 0))
            {
                var me = ((UserModel)storeService.Get(CommonKeys.LoggedUser.ToString())).Id;
                var contact = newChat.Users.Where(u => u.Id != me).First();//accesing  .Id prop throws null reference exception when index out of range
                newChat.Contact = contact.UserName;
                OnlineContacts.Add(newChat);
            }
            //else { MessageBox.Show("Couldnt find user"); }
        }

        private void FetchUserHandler()
        {
            LoggedUser = storeService.Get(CommonKeys.LoggedUser.ToString());
            DisplayedUser = $"{LoggedUser.UserName} (#{LoggedUser.Id})";
        }

        private void UpdateChatInStore() => storeService.Add(CommonKeys.CurrentChat.ToString(), SelectedChat);
    }
}
