using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using tWpfMashUp_v0._0._1.Core;
using System.Collections.Generic;
using tWpfMashUp_v0._0._1.Sevices;
using System.Collections.ObjectModel;
using tWpfMashUp_v0._0._1.MVVM.Models;

namespace tWpfMashUp_v0._0._1.MVVM.ViewModels
{
    public class ChatAppViewModel : ObservableObject
    {
        //fields-services
        private readonly StoreService store;
        private readonly SignalRListinerService signalRListinerService;
        private readonly AuthenticationService authenticationService;
        private readonly ChatsService chatsService;

        //full props
        private User loggedUser;
        public User LoggedUser { get => loggedUser; set { loggedUser = value; onProppertyChange(); } }

        private User selectedUser;
        public User SelectedUser { get => selectedUser; set { selectedUser = value; onProppertyChange(); UpdateChatInStore(); } }
        private string displayedUser;
        public string DisplayedUser { get { return displayedUser; } set { displayedUser = value; onProppertyChange(); } }
        private string message;
        public string BindingTest { get => message; set { message = value; onProppertyChange(); } }
        //commands
        public RelayCommand OnSelectionChangedCommand { get; set; }

        public ObservableCollection<User> OnlineContacts { get; set; }
        public ObservableCollection<User> OfflineContacts { get; set; }

        public ChatAppViewModel(StoreService storeService, ChatsService chatsService, AuthenticationService authenticationService, SignalRListinerService signalRListinerService)
        {
            this.signalRListinerService = signalRListinerService;
            this.authenticationService = authenticationService;
            this.chatsService = chatsService;
            this.store = storeService;
            OfflineContacts = new ObservableCollection<User>();
            OnlineContacts = new ObservableCollection<User>();
            authenticationService.LoggingIn += (s, e) => FetchUserHandler();
            signalRListinerService.ContactLogged += OnContactLogged;
            OnSelectionChangedCommand = new RelayCommand(o => HandleSelectionChanged(o as SelectionChangedEventArgs));
            //authenticationService.UsersFetched += (s, e) => UpdateUsersList();
        }
        private void FetchUserHandler()
        {
            LoggedUser = store.Get(CommonKeys.LoggedUser.ToString()) as User;
            DisplayedUser = $"{LoggedUser.UserName} (#{LoggedUser.Id})";
            FetchAllOnlineContacts();
        }
        private async void FetchAllOnlineContacts()
        {
            await authenticationService.FetchAllLoggedUsers();
            UpdateUsersList();
        }
        private void UpdateUsersList()
        {
            var users = store.Get(CommonKeys.Contacts.ToString()) as List<User>;
            if (users.Any())
            {
                OnlineContacts = new ObservableCollection<User>();
                OfflineContacts = new ObservableCollection<User>();
            }
            foreach (var u in users)
            {
                if (u.IsConnected == Status.Online) OnlineContacts.Add(u);
                else OfflineContacts.Add(u);
            }
        }
       
        private void OnContactLogged(object sender, System.EventArgs e)
        {
            var args = e as ContactLoggedEventArgs;
            if (args.IsLoggedIn) OnContactLoggedIn(args.User);
            else OnContactLoggedOut(args.User);
        }
        private void OnContactLoggedIn(User user)
        {
            //var me = (store.Get(CommonKeys.LoggedUser.ToString())) as User;
            OnlineContacts.Add(user);
            OfflineContacts.Remove(user);
        }
        private void OnContactLoggedOut(User user)
        {
            OfflineContacts.Add(user);
            OnlineContacts.Remove(user);
        }

        public void HandleSelectionChanged(SelectionChangedEventArgs selectionChangedEventArgs)
        {
            try
            {
                if (selectionChangedEventArgs.RemovedItems != null && selectionChangedEventArgs.RemovedItems.Count > 0)
                    store.Remove(CommonKeys.CurrentChat.ToString());
                if (selectionChangedEventArgs.AddedItems != null && selectionChangedEventArgs.AddedItems.Count > 0)
                {
                    var newCurrentChat = selectionChangedEventArgs.AddedItems[0] as Chat;
                    store.Add(CommonKeys.CurrentChat.ToString(), newCurrentChat);
                }
                store.InformContactChanged(selectionChangedEventArgs.Source, selectionChangedEventArgs);
            }
            catch { }
        }
       
        private void UpdateChatInStore() => store.Add(CommonKeys.WithUser.ToString(), SelectedUser);
    }
}
