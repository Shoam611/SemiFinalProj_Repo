﻿using System.Linq;
using System.Windows.Controls;
using tWpfMashUp_v0._0._1.Core;
using System.Collections.Generic;
using tWpfMashUp_v0._0._1.Sevices;
using System.Collections.ObjectModel;
using tWpfMashUp_v0._0._1.MVVM.Models;
using System.Windows.Threading;
using System.Windows;

namespace tWpfMashUp_v0._0._1.MVVM.ViewModels
{
    public class ChatAppViewModel : ObservableObject
    {
        //fields
        private readonly StoreService store;
        private readonly SignalRListenerService signalRListinerService;
        private readonly AuthenticationService authenticationService;
        private readonly ChatsService chatsService;
        //full props
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
        //commands
        public RelayCommand OnSelectionChangedCommand { get; set; }

        public ObservableCollection<User> OnlineContacts { get; set; }
        public ObservableCollection<User> OfflineContacts { get; set; }

        public ChatAppViewModel(StoreService store, ChatsService chatsService, AuthenticationService authenticationService, SignalRListenerService signalRListinerService)
        {
            this.authenticationService = authenticationService;
            this.chatsService = chatsService;
            this.store = store;
            this.signalRListinerService = signalRListinerService;
            OfflineContacts = new ObservableCollection<User>();
            OnlineContacts = new ObservableCollection<User>();
            this.authenticationService.LoggingIn += (s, e) => FetchUserHandler();
            this.signalRListinerService.ContactLogged += OnContactLogged;
            OnSelectionChangedCommand = new RelayCommand(o => HandleSelectionChanged(o as SelectionChangedEventArgs));
            // authenticationService.UserFetch
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
            Dispatcher.CurrentDispatcher.Invoke(() => UpdateUsersList());
        }
        private void UpdateUsersList()
        {
            var users = store.Get(CommonKeys.Contacts.ToString()) as List<User>;
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
            if (args.IsLoggedIn) Dispatcher.CurrentDispatcher.Invoke(() => OnContactLoggedIn(args.User));
            else Dispatcher.CurrentDispatcher.Invoke(() => OnContactLoggedOut(args.User));
        }

        private void OnContactLoggedIn(User user)
        {
            if (!OnlineContacts.Where(u => u.Id == user.Id).Any())
            {
                Dispatcher.CurrentDispatcher.Invoke(() =>
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
                Dispatcher.CurrentDispatcher.Invoke(() =>
                    {
                        OfflineContacts.Add(user);
                        OnlineContacts.Remove(OnlineContacts.FirstOrDefault(u => u.Id == user.Id));
                    });
            }
        }

        public void HandleSelectionChanged(SelectionChangedEventArgs selectionChangedEventArgs)
        {
            try
            {
                if (selectionChangedEventArgs.RemovedItems != null && selectionChangedEventArgs.RemovedItems.Count > 0)
                {
                    store.Remove(CommonKeys.CurrentChat.ToString());
                    store.Remove(CommonKeys.WithUser.ToString());
                }
                if (selectionChangedEventArgs.AddedItems != null && selectionChangedEventArgs.AddedItems.Count > 0)
                {
                    var newCurrentUser = selectionChangedEventArgs.AddedItems[0] as User;
                    store.Add(CommonKeys.WithUser.ToString(), newCurrentUser);
                    //create new chat
                    chatsService.CreateChatIfNotExistAsync(newCurrentUser);
                }
                store.InformContactChanged(selectionChangedEventArgs.Source, selectionChangedEventArgs);
            }
            catch { }
        }

        private void UpdateChatInStore() => store.Add(CommonKeys.WithUser.ToString(), SelectedUser);
    }
}
