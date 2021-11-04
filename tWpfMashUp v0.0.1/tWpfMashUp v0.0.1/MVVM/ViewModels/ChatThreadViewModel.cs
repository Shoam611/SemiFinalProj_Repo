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
        private readonly SignalRListenerService listenerService;

        private string currentContact;
        public string CurrentContact
        {
            get => currentContact;
            set { currentContact = value; onProppertyChange(); }
        }

        public RelayCommand AddMessageCommand { get; set; }
        private ObservableCollection<Massage> messages;
        public ObservableCollection<Massage> Messages { get => messages; set { messages = value; onProppertyChange(); } }

        private string message;
        public string Message { get => message; set { message = value; onProppertyChange(); } }

        public ChatThreadViewModel(MessagesService messagesService, StoreService storeService, SignalRListenerService listenerService)
        {
            CurrentContact = "";
            this.storeService = storeService;
            this.listenerService = listenerService;
            this.messagesService = messagesService;
            Messages = new ObservableCollection<Massage>();
            AddMessageCommand = new RelayCommand((o) => AddMessageHandler());
            storeService.CurrentContactChanged += OnCurrentContactChanged;
            this.listenerService.MessageRecived += OnMessageRecived;
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
                    if (storeService.HasKey(CommonKeys.CurrentChat.ToString()))
                    {
                        var cChat = (storeService.Get(CommonKeys.CurrentChat.ToString()) as Chat);
                        if (cChat.Messages != null)
                            Messages = new ObservableCollection<Massage>(cChat.Messages);
                        else Messages.Clear();

                    }
                    CurrentContact = (args.AddedItems[0] as User).UserName;
                }
            }
            catch { }
        }
        private void OnMessageRecived(object sender, MessageRecivedEventArgs eventArgs)
        {
            var currentChatId = (storeService.Get(CommonKeys.CurrentChat.ToString()) as Chat).Id;
           if (eventArgs.ChatID == currentChatId)
                Messages.Add(eventArgs.Massage);
            else
            {

            }
        }
        private async void AddMessageHandler()
        {
            var isSuccesfull = await messagesService.CallServerToAddMessage(Message);
            if (isSuccesfull)
            Message = "";
        }

       // public void ChatChangedHandler(RoutedEventArgs routedEventArgs) => Messages = new ObservableCollection<Massage>(((Chat)storeService.Get(CommonKeys.CurrentChat.ToString())).Messages);
    }
}
