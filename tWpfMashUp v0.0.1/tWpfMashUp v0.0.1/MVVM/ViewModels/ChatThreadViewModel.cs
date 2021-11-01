using tWpfMashUp_v0._0._1.MVVM.Models;
using System.Collections.ObjectModel;
using tWpfMashUp_v0._0._1.Core;
using tWpfMashUp_v0._0._1.Sevices;
using System.Windows;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace tWpfMashUp_v0._0._1.MVVM.ViewModels
{
    public class ChatThreadViewModel : ObservableObject
    {
        private readonly SignalRListinerService signalRListinerService;
        private readonly MessagesService messagesService;
        private readonly StoreService storeService;
        public RelayCommand AddMessageCommand { get; set; }

        private ObservableCollection<Massage> messages;
        public ObservableCollection<Massage> Messages { get => messages; set { messages = value; onProppertyChange(); } }

        private string message;
        public string Message { get => message; set { message = value; onProppertyChange(); } }

        private string currentContact;
        public string CurrentContact { get => currentContact; set { currentContact = value; onProppertyChange(); } }


        public ChatThreadViewModel(MessagesService messagesService, StoreService storeService, SignalRListinerService signalRListinerService)
        {
            this.storeService = storeService;
            this.signalRListinerService = signalRListinerService;
            this.messagesService = messagesService;
            Messages = new ObservableCollection<Massage>();
            AddMessageCommand = new RelayCommand((o) => AddMessageHandler());
        }

        private async void AddMessageHandler()
        {

            var isSuccesfull = await messagesService.CallServerToAddMessage(Message);
            if (isSuccesfull)
                Messages.Add(new Massage { Content = Message, Date = DateTime.Now, Name = storeService.Get(CommonKeys.LoggedUser.ToString()).UserName });
            Message = "";
        }

        public void ChatChangedHandler(RoutedEventArgs routedEventArgs)
        {
            Messages = new ObservableCollection<Massage>(((Chat)storeService.Get(CommonKeys.CurrentChat.ToString())).Messages);
        }
    }
}
