using System.Linq;
using System.Windows.Controls;
using tWpfMashUp_v0._0._1.Core;
using System.Collections.Generic;
using tWpfMashUp_v0._0._1.Sevices;
using System.Collections.ObjectModel;
using tWpfMashUp_v0._0._1.MVVM.Models;
using System.Windows.Threading;

namespace tWpfMashUp_v0._0._1.MVVM.ViewModels
{
    public class ChatAppViewModel : ObservableObject
    {
        //fields-services
        private readonly StoreService store;
        private readonly AuthenticationService authenticationService;
        SignalRListinerService signalRListinerService;
        //full props
        private User loggedUser;
        public User LoggedUser { get => loggedUser; set { loggedUser = value; onProppertyChange(); } }

        private User selectedUser;
        public User SelectedUser { get => selectedUser; set { selectedUser = value; onProppertyChange(); UpdateChatInStore(); } }
        private string displayedUser;
        public string DisplayedUser { get => displayedUser; set { displayedUser = value; onProppertyChange(); } }
        private string bindingTest;       
        public string BindingTest { get => bindingTest; set { bindingTest = value; onProppertyChange(); } }
        //commands
        public RelayCommand OnSelectionChangedCommand { get; set; }

        public ObservableCollection<User> OnlineContacts { get; set; }
        public ObservableCollection<User> OfflineContacts { get; set; }

        public ChatAppViewModel(StoreService store, ChatsService chatsService, AuthenticationService authenticationService, SignalRListinerService signalRListinerService)
        {
            this.authenticationService = authenticationService;
            this.signalRListinerService = signalRListinerService;
            this.store = store;
            this.authenticationService.LoggingIn += (s, e) => FetchUserHandler();
            this.signalRListinerService.ContactLogged += OnContactLogged;
            OnlineContacts = new ObservableCollection<User>();
            OfflineContacts = new ObservableCollection<User>();
            OnSelectionChangedCommand = new RelayCommand(o => HandleSelectionChanged(o as SelectionChangedEventArgs));
           // authenticationService.UserFetch
        }
        private  void FetchUserHandler()
        {
            LoggedUser = store.Get(CommonKeys.LoggedUser.ToString()) as User;
            DisplayedUser = $"{LoggedUser.UserName} (#{LoggedUser.Id})";
           FetchAllOnlineContacts();
                       
        }
        private async void FetchAllOnlineContacts()
        {
            await authenticationService.FetchAllLoggedUsers();
            Dispatcher.CurrentDispatcher.Invoke(()=>UpdateUsersList());
        }

        private void UpdateUsersList()
        {
            var users = store.Get(CommonKeys.Contacts.ToString()) as List<User>;
            if (!users.Any()) return;
                                        
            foreach (var u in users)
            {
                if (u.IsConnected == Status.Online) OnlineContacts.Add(u);
                else OfflineContacts.Add(u);
            }
            onProppertyChange();
        }
       
        private void OnContactLogged(object sender, System.EventArgs e)
        {
            var args = e as ContactLoggedEventArgs;
            if (args.IsLoggedIn) Dispatcher.CurrentDispatcher.Invoke(()=> OnContactLoggedIn(args.User));
            else Dispatcher.CurrentDispatcher.Invoke(()=>OnContactLoggedOut(args.User));
        }
        private void OnContactLoggedIn(User user)
        {
            OnlineContacts.Add(user);
            OfflineContacts.Remove(user);
        }
        private void OnContactLoggedOut(User user)
        {           
                OfflineContacts.Add(user);
                OnlineContacts.Remove(OnlineContacts.FirstOrDefault(u => u.Id == user.Id));           
        }

        public void HandleSelectionChanged(SelectionChangedEventArgs selectionChangedEventArgs)
        {
            //go to service and create chat if not exist
            //call chat thread update
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
