using System.Linq;
using System.Windows.Controls;
using tWpfMashUp_v0._0._1.Core;
using System.Collections.Generic;
using tWpfMashUp_v0._0._1.Sevices;
using System.Collections.ObjectModel;
using tWpfMashUp_v0._0._1.MVVM.Models;
using System.Windows.Threading;
using System;
using System.Windows;
using tWpfMashUp_v0._0._1.Assets.Components.CustomModal;

namespace tWpfMashUp_v0._0._1.MVVM.ViewModels
{
    public class ChatAppViewModel : ObservableObject
    {
        //Fields
        private readonly StoreService storeService;
        private readonly SignalRListenerService signalRListinerService;
        private readonly AuthenticationService authenticationService;
        private readonly ChatsService chatsService;
        private readonly GameLogicService gameService;

        //Full Props
        private User loggedUser;
        public User LoggedUser { get => loggedUser; set { loggedUser = value; onProppertyChange(); } }

        private Chat selectedChat;
        public Chat SelectedChat { get => selectedChat; set { selectedChat = value; onProppertyChange(); UpdateChatInStore(); } }

        private string displayedUser;
        public string DisplayedUser { get { return displayedUser; } set { displayedUser = value; onProppertyChange(); } }

        private User selectedUser;
        public User SelectedUser { get => selectedUser; set { selectedUser = value; onProppertyChange(); UpdateChatInStore(); } }

        private string bindingTest;
        public string BindingTest { get => bindingTest; set { bindingTest = value; onProppertyChange(); } }

        //Commands
        public RelayCommand OnSelectionChangedCommand { get; set; }
        public RelayCommand OnInviteToGameCommand { get; set; }

        //Ui Collections
        public ObservableCollection<User> OnlineContacts { get; set; }
        public ObservableCollection<User> OfflineContacts { get; set; }

        public ChatAppViewModel(StoreService store, ChatsService chatsService,
            AuthenticationService authenticationService, SignalRListenerService signalRListinerService, GameLogicService gameService)
        {
            this.storeService = store;
            this.gameService = gameService;
            this.chatsService = chatsService;
            this.authenticationService = authenticationService;
            this.signalRListinerService = signalRListinerService;
            OfflineContacts = new ObservableCollection<User>();
            OnlineContacts = new ObservableCollection<User>();

            OnSelectionChangedCommand = new RelayCommand(o => HandleSelectionChanged(o as SelectionChangedEventArgs));
            OnInviteToGameCommand = new RelayCommand((o) => InviteToGame());

            this.authenticationService.LoggingIn += (s, e) => FetchUserHandler();
            this.signalRListinerService.ContactLogged += OnContactLogged;
            this.signalRListinerService.MessageRecived += OnMassageRecived;
            this.signalRListinerService.UserInvitedToGame += OnGameInvitation;
        }

        private void OnGameInvitation(object sender, UserInvitedEventArgs eventArgs)
        {
            var mb = Modal.ShowModal($"{eventArgs.User.UserName} You were invited to a game!", "Game Invitation", "Accept", "Deny", "Cancel");
            if(mb == "Accept")
            {
                //switch to game view
                Modal.ShowModal("Good luck!");
            }
        }

        private async void InviteToGame()
        {
            await gameService.CallServerForOtherUserInvite();
        }

        private void OnMassageRecived(object sender, MessageRecivedEventArgs eventArgs)
        {
            if (storeService.HasKey(CommonKeys.CurrentChat.ToString()))
            {
                var c = storeService.Get(CommonKeys.CurrentChat.ToString()) as Chat;
                if (eventArgs.ChatId == c.Id) { return; }
            }
            var contacts = (storeService.Get(CommonKeys.Contacts.ToString()) as List<User>);
            var contact = contacts.First(u => u.UserName == eventArgs.Massage.Name);
            contact.HasUnreadMessage = true;
            OnlineContacts.Remove(OnlineContacts.First(u => u.Id == contact.Id));
            OnlineContacts.Add(contact);
            //OnlineContacts= new ObservableCollection<User>(contacts);
        }

        private void FetchUserHandler()
        {
            LoggedUser = storeService.Get(CommonKeys.LoggedUser.ToString()) as User;
            DisplayedUser = $"{LoggedUser.UserName} (#{LoggedUser.Id})";
            FetchAllOnlineContacts();
        }

        private async void FetchAllOnlineContacts()
        {
            await authenticationService.FetchAllLoggedUsers();
            App.Current.Dispatcher.Invoke(() => UpdateUsersList());
        }

        private void UpdateUsersList()
        {
            var users = storeService.Get(CommonKeys.Contacts.ToString()) as List<User>;
            if (!users.Any()) return;
            OnlineContacts.Clear();
            OfflineContacts.Clear();
            foreach (var u in users)
            {
                if (u.IsConnected == Status.Online) OnlineContacts.Add(u);
                else OfflineContacts.Add(u);
            }
        }

        private void OnContactLogged(object sender, System.EventArgs e)
        {
            var args = e as ContactLoggedEventArgs;
            if (args.IsLoggedIn) App.Current.Dispatcher.Invoke(() => OnContactLoggedIn(args.User));
            else App.Current.Dispatcher.Invoke(() => OnContactLoggedOut(args.User));
        }

        private void OnContactLoggedIn(User user)
        {
            if (!OnlineContacts.Where(u => u.Id == user.Id).Any())
            {
                App.Current.Dispatcher.Invoke(() =>
                  {
                      OnlineContacts.Add(user);
                      OfflineContacts.Remove(OfflineContacts.FirstOrDefault(u => u.Id == user.Id));
                  });

            }
        }

        private void OnContactLoggedOut(User user)
        {
            if (!OfflineContacts.Where(u => u.Id == user.Id).Any())
            {
                App.Current.Dispatcher.Invoke(() =>
                    {
                        OfflineContacts.Add(user);
                        OnlineContacts.Remove(OnlineContacts.FirstOrDefault(u => u.Id == user.Id));
                    });
            }
        }

        public async void HandleSelectionChanged(SelectionChangedEventArgs selectionChangedEventArgs)
        {
            try
            {
                if (selectionChangedEventArgs.RemovedItems != null && selectionChangedEventArgs.RemovedItems.Count > 0)
                {
                    storeService.Remove(CommonKeys.CurrentChat.ToString());
                    storeService.Remove(CommonKeys.WithUser.ToString());
                }
                if (selectionChangedEventArgs.AddedItems != null && selectionChangedEventArgs.AddedItems.Count > 0)
                {
                    var newCurrentUser = selectionChangedEventArgs.AddedItems[0] as User;
                    storeService.Add(CommonKeys.WithUser.ToString(), newCurrentUser);
                    await chatsService.CreateChatIfNotExistAsync(newCurrentUser);
                    if (newCurrentUser.HasUnreadMessage)
                    {
                        newCurrentUser.HasUnreadMessage = false;
                        var user = OnlineContacts.First(u => u.Id == newCurrentUser.Id);
                        user.HasUnreadMessage = false;
                        OnlineContacts.Remove(user);
                        OnlineContacts.Add(newCurrentUser);
                        SelectedUser = user;
                    }

                }
                storeService.InformContactChanged(selectionChangedEventArgs.Source, selectionChangedEventArgs);
            }
            catch { }
        }

        private void OnMassageRecived(object sender, MessageRecivedEventArgs eventArgs)
        {
            if (store.HasKey(CommonKeys.CurrentChat.ToString()))
            {
                var c = store.Get(CommonKeys.CurrentChat.ToString()) as Chat;
                if (eventArgs.ChatId == c.Id) { return; }                
            }
           var contacts = (store.Get(CommonKeys.Contacts.ToString()) as List<User>);
            var contact = contacts.First(u => u.UserName == eventArgs.Massage.Name);
            contact.HasUnreadMessage = true;//false
            OnlineContacts.Remove(OnlineContacts.First(u=>u.Id==contact.Id));
            OnlineContacts.Insert(0, contact);                       
        }    

    }
}
