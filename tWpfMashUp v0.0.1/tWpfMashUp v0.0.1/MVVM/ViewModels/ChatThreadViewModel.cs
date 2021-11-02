using tWpfMashUp_v0._0._1.MVVM.Models;
using System.Collections.ObjectModel;
using tWpfMashUp_v0._0._1.Core;
using tWpfMashUp_v0._0._1.Sevices;
using System.Windows;
using System;
using System.Linq;
using System.Windows.Controls;

namespace tWpfMashUp_v0._0._1.MVVM.ViewModels
{
    public class ChatThreadViewModel : ObservableObject
    {
        private readonly MessagesService messagesService;
        private readonly StoreService storeService;

        private string currentUser;
        public string CurrentContact
        {
            get => currentUser;
            set { currentUser = value; onProppertyChange(); }
        }

        public RelayCommand AddMessageCommand { get; set; }
        private ObservableCollection<Massage> messages;
        public ObservableCollection<Massage> Messages { get => messages; set { messages = value; onProppertyChange(); } }

        private string message;
        public string Message { get => message; set { message = value; onProppertyChange(); } }

        public ChatThreadViewModel(MessagesService messagesService, StoreService storeService)
        {
            CurrentContact = "";
            this.storeService = storeService;
            this.messagesService = messagesService;
            Messages = new ObservableCollection<Massage>();
            AddMessageCommand = new RelayCommand((o) => AddMessageHandler());
            storeService.CurrentContactChanged += OnCurrentContactChanged;
        }

        private void OnCurrentContactChanged(object sender, EventArgs e)
        {
            try
            {
                var args = e as SelectionChangedEventArgs;
                if (args.RemovedItems != null && args.RemovedItems.Count > 0)
                    CurrentContact = " ";
                if (args.AddedItems != null && args.AddedItems.Count > 0)
                {
                    var newChat = args.AddedItems[0] as Chat;
                    CurrentContact = newChat.Contact;
                }
            }
            catch { }
        }

        private async void AddMessageHandler()
        {
            if (storeService.HasKey(CommonKeys.CurrentChat.ToString()))
            {
                var isSuccesfull = await messagesService.CallServerToAddMessage(Message);
                if (isSuccesfull)
                    Message = "";
            }
            // Messages.Add(new Massage { Content = Message, Date = DateTime.Now, Name = storeService.Get(CommonKeys.LoggedUser.ToString()).UserName });
        }

        public void ChatChangedHandler(RoutedEventArgs routedEventArgs) => Messages = new ObservableCollection<Massage>(((Chat)storeService.Get(CommonKeys.CurrentChat.ToString())).Messages);
    }
}
