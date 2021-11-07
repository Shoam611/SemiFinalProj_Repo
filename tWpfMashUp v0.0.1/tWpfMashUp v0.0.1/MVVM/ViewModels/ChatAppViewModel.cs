﻿using System.Linq;
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
        //fields
        #region fields
        private readonly StoreService store;
        private readonly SignalRListenerService signalRListinerService;
        private readonly AuthenticationService authenticationService;
        private readonly ChatsService chatsService; 
        #endregion
        //full props
        #region full props
        private User loggedUser;
        public User LoggedUser { get => loggedUser; set { loggedUser = value; onProppertyChange(); } }

        private string displayedUser;
        public string DisplayedUser { get { return displayedUser; } set { displayedUser = value; onProppertyChange(); } }

        private User selectedUser;
        public User SelectedUser { get => selectedUser; set { selectedUser = value; onProppertyChange();} }
        #endregion
        //commands and props
        #region props
        public RelayCommand OnSelectionChangedCommand { get; set; }
        public ObservableCollection<User> OnlineContacts { get; set; }
        public ObservableCollection<User> OfflineContacts { get; set; } 
        #endregion

        public ChatAppViewModel(StoreService store, ChatsService chatsService, AuthenticationService authenticationService, SignalRListenerService signalRListinerService)
        {
            this.authenticationService = authenticationService;
            this.chatsService = chatsService;
            this.store = store;
            this.signalRListinerService = signalRListinerService;
            OfflineContacts = new ObservableCollection<User>();
            OnlineContacts = new ObservableCollection<User>();
            OnSelectionChangedCommand = new RelayCommand(o => HandleSelectionChanged(o as SelectionChangedEventArgs));
            this.authenticationService.LoggingIn += (s, e) => FetchUserHandler();
            this.signalRListinerService.ContactLogged += OnContactLogged;
            this.signalRListinerService.MessageRecived += OnMassageRecived;
        }

        private void FetchUserHandler()
        {
            LoggedUser = store.Get(CommonKeys.LoggedUser.ToString()) as User;
            DisplayedUser = $"{LoggedUser.UserName} (#{LoggedUser.Id})";
            FetchAllContacts();
        }
     
        private async void FetchAllContacts()
        {
            await authenticationService.FetchAllLoggedUsers();
            App.Current.Dispatcher.Invoke(() => UpdateUsersList());
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
            if (args.IsLoggedIn) App.Current.Dispatcher.Invoke(() => OnContactLoggedIn(args.User));
            else App.Current.Dispatcher.Invoke(() => OnContactLoggedOut(args.User));
            //System.NullReferenceException: 'Object reference not set to an instance of an object.'
            //System.Windows.Application.Current.get returned null.
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
                //System.NotSupportedException: 'This type of CollectionView does not support changes to its SourceCollection from a thread different from the Dispatcher thread.'

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
                    store.Remove(CommonKeys.CurrentChat.ToString());
                    store.Remove(CommonKeys.WithUser.ToString());
                }
                if (selectionChangedEventArgs.AddedItems != null && selectionChangedEventArgs.AddedItems.Count > 0)
                {
                    var newCurrentUser = selectionChangedEventArgs.AddedItems[0] as User;
                    store.Add(CommonKeys.WithUser.ToString(), newCurrentUser);
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
                store.InformContactChanged(selectionChangedEventArgs.Source, selectionChangedEventArgs);
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
            OnlineContacts.Add(contact);                        
            //OnlineContacts= new ObservableCollection<User>(contacts);
        }    

    }
}
