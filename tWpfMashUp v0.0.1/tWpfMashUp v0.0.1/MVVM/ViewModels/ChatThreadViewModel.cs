using tWpfMashUp_v0._0._1.MVVM.Models;
using System.Collections.ObjectModel;
using tWpfMashUp_v0._0._1.Core;
using tWpfMashUp_v0._0._1.Sevices;

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
            var msg = await messagesService.CallServerToAddMessage(Message);
            AddMessageToUI(msg);
            Message = "";
        }

        private void AddMessageToUI(Message msg)
        {
            Messages.Add(msg);
        }
    }
}
