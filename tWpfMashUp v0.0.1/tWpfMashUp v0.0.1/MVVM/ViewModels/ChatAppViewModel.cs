using System.Linq;
using tWpfMashUp_v0._0._1.Core;
using tWpfMashUp_v0._0._1.Sevices;
using System.Collections.ObjectModel;
using tWpfMashUp_v0._0._1.MVVM.Models;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;

namespace tWpfMashUp_v0._0._1.MVVM.ViewModels
{
    public class ChatAppViewModel : ObservableObject
    {
        //fields
        StoreService store;
        private SignalRListinerService signalRListinerService;
        private AuthenticationService authenticationService;
        ChatsService chatsService;

        //full props
        User loggedUser;
        public User LoggedUser { get => loggedUser; set { loggedUser = value; onProppertyChange(); } }
        Chat selectedChat;
        public Chat SelectedChat { get => selectedChat; set { selectedChat = value; onProppertyChange(); UpdateChatInStore(); } }
        private string displayedUser;
        public string DisplayedUser { get { return displayedUser; } set { displayedUser = value; onProppertyChange(); } }

        private string message;
        public string BindingTest { get => message; set { message = value; onProppertyChange(); } }
        //props
        public RelayCommand GoToGameCommand { get; set; }
        public RelayCommand FetchUserCommand { get; set; }
        public RelayCommand GetRandomChatCommand { get; set; }
        public RelayCommand OnSelectionChangedCommand { get; set; }

        public ObservableCollection<Chat> OnlineContacts { get; set; }
        public ObservableCollection<Chat> OfflineContacts { get; set; }

        public ChatAppViewModel(StoreService storeService, ChatsService chatsService, AuthenticationService authenticationService, SignalRListinerService signalRListinerService)
        {
            this.signalRListinerService = signalRListinerService;
            this.authenticationService = authenticationService;
            this.chatsService = chatsService;
            this.store = storeService;
            OfflineContacts = new ObservableCollection<Chat>();
            OnlineContacts = new ObservableCollection<Chat>();
            FetchUserCommand = new RelayCommand(o => FetchUserHandler());
            GetRandomChatCommand = new RelayCommand(o => GetChat());
            authenticationService.LoggingIn += (s, e) => FetchUserHandler();
            signalRListinerService.ContactLogged += OnContactLogged;
            // OnSelectionChangedCommand = new RelayCommand(o => HandleSelectionChanged(o as RoutedEventArgs));
        }

        private void OnContactLogged(object sender, System.EventArgs e)
        {
            var args = e as ContactLoggedEventArgs;
            if (args.IsLoggedIn) OnContactLoggedIn(args.User);
            
            else OnContactLoggedOut(args.User);
        }

        private void OnContactLoggedIn(User user)
        {
            var c = OfflineContacts.FirstOrDefault(c => c.ContactId == user.Id);
            if (c != null) OfflineContacts.Remove(c);
            else
                c = new Chat
                {
                    Contact = user.UserName,
                    Messages = new List<Massage>(),
                    ContactId = user.Id,
                    Users = new List<User> { user, store.Get(CommonKeys.LoggedUser.ToString()) }
                };
            OnlineContacts.Add(c);
        }
        private void OnContactLoggedOut(User user)
        {
            var chatToRemove = OnlineContacts.FirstOrDefault(c => c.ContactId == user.Id);
            if (chatToRemove != null)
            {
                if (chatToRemove.Messages != null && chatToRemove.Messages.Any())
                    OfflineContacts.Add(chatToRemove);
                else
                {
                    var contacts = store.Get(CommonKeys.Contacts.ToString()) as List<User>;
                    if (contacts == null) contacts = new List<User>();
                    contacts.Remove(user);
                    store.Add(CommonKeys.Contacts.ToString(), contacts);
                }
                OnlineContacts.Remove(chatToRemove);
            }
        }

        public void HandleSelectionChanged(SelectionChangedEventArgs selectionChangedEventArgs)
        {
            var newCurrentChat = (Chat)selectionChangedEventArgs.AddedItems[0];
            store.Add(CommonKeys.CurrentChat.ToString(), newCurrentChat);
        }

        private async void GetChat()
        {
            var isParsed = int.TryParse(BindingTest, out int res);
            if (!isParsed) { MessageBox.Show("NaN"); return; }
            var newChat = await chatsService.GetChatAsync(res);
            if (newChat != null && !(OnlineContacts.Where(c => c.Id == newChat.Id).ToList().Count > 0))
            {
                var me = ((User)store.Get(CommonKeys.LoggedUser.ToString())).Id;
                var contact = newChat.Users.Where(u => u.Id != me).First();//accesing  .Id prop throws null reference exception when index out of range
                newChat.Contact = contact.UserName;
                OnlineContacts.Add(newChat);
            }
            //else { MessageBox.Show("Couldnt find user"); }
        }

        private void FetchUserHandler()
        {
            LoggedUser = store.Get(CommonKeys.LoggedUser.ToString());
            DisplayedUser = $"{LoggedUser.UserName} (#{LoggedUser.Id})";
        }

        private void UpdateChatInStore() => store.Add(CommonKeys.CurrentChat.ToString(), SelectedChat);
    }
}
