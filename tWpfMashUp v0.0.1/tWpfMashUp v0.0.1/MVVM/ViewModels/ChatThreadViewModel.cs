using tWpfMashUp_v0._0._1.MVVM.Models;
using System.Collections.ObjectModel;
using tWpfMashUp_v0._0._1.Core;
using tWpfMashUp_v0._0._1.Sevices;

namespace tWpfMashUp_v0._0._1.MVVM.ViewModels
{
    public class ChatThreadViewModel : ObservableObject
    {
        readonly MessagesService messagesService;
        public RelayCommand AddMessageCommand { get; set; }

        private ObservableCollection<Message> messages;
        public ObservableCollection<Message> Messages { get => messages; set { messages = value; onProppertyChange(); } }

        private string message;

        public string Message { get => message; set { message = value; onProppertyChange(); } }
        public ChatThreadViewModel(MessagesService messagesService) //
        {
            this.messagesService = messagesService;
            Messages = new ObservableCollection<Message>();
            AddMessageCommand = new RelayCommand((o) => AddMessageHandler());
        }

        private async void AddMessageHandler()
        {
            var msg = await messagesService.CallServerToAddMessage(Message);
            NewMethod(msg);
            Message = "";
        }

        private void NewMethod(Message msg)
        {
            Messages.Add(msg);
        }
    }
}
