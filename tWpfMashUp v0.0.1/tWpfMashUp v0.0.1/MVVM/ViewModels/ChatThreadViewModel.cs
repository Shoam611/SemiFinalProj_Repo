using tWpfMashUp_v0._0._1.MVVM.Models;
using System.Collections.ObjectModel;
using tWpfMashUp_v0._0._1.Core;
using tWpfMashUp_v0._0._1.Sevices;
using System.Windows;
using System;
using System.Linq;

namespace tWpfMashUp_v0._0._1.MVVM.ViewModels
{
    public class ChatThreadViewModel : ObservableObject
    {
        readonly MessagesService messagesService;
        readonly StoreService storeService;
        public RelayCommand AddMessageCommand { get; set; }

        private ObservableCollection<Message> messages;
        public ObservableCollection<Message> Messages { get => messages; set { messages = value; onProppertyChange(); } }

        private string message;
        public string Message { get => message; set { message = value; onProppertyChange(); } }

        public ChatThreadViewModel(MessagesService messagesService, StoreService storeService) //
        {
            this.storeService = storeService;
            this.messagesService = messagesService;
            Messages = new ObservableCollection<Message>();
            AddMessageCommand = new RelayCommand((o) => AddMessageHandler());
        }
        private async void AddMessageHandler()
        {
            
           var isSuccesfull = await messagesService.CallServerToAddMessage(Message);
            if(isSuccesfull)
            Messages.Add(new Message { Content = Message,Date=DateTime.Now,Name=storeService.Get(CommonKeys.LoggedUser.ToString()).UserName });
            Message = "";
        }

        public void ChatChangedHandler(RoutedEventArgs routedEventArgs)
        {
            Messages = new ObservableCollection<Message>(((Chat)storeService.Get(CommonKeys.CurrentChat.ToString())).Messages);
        }      
    }
}
