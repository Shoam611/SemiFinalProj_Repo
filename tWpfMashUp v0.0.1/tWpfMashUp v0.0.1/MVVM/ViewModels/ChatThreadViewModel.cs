using tWpfMashUp_v0._0._1.MVVM.Models;
using System.Collections.ObjectModel;
using tWpfMashUp_v0._0._1.Core;
using tWpfMashUp_v0._0._1.Sevices;
using System.Windows;
using System;
using System.Linq;
using System.Windows.Controls;
using tWpfMashUp_v0._0._1.Extentions;

namespace tWpfMashUp_v0._0._1.MVVM.ViewModels
{
    public class ChatThreadViewModel : ObservableObject
    {
        private readonly MessagesService messagesService;
        private readonly StoreService storeService;
        private readonly SignalRListenerService listenerService;
        private readonly ChatsService chatService;

        private string currentContact;
        public string CurrentContact { get => currentContact; set { currentContact = value; onProppertyChange(); } }

        private ObservableCollection<Massage> messages;
        public ObservableCollection<Massage> Messages { get => messages; set { messages = value; onProppertyChange(); } }

        private string message;
        public string Message { get => message; set { message = value; onProppertyChange(); } }

        public RelayCommand AddMessageCommand { get; set; }

        public ChatThreadViewModel(MessagesService messagesService, ChatsService chatsService, StoreService storeService, SignalRListenerService listenerService)
        {
            chatService = chatsService;
            this.storeService = storeService;
            this.listenerService = listenerService;
            this.messagesService = messagesService;
            CurrentContact = "";
            Messages = new ObservableCollection<Massage>();
            AddMessageCommand = new RelayCommand((o) => AddMessageHandler());
            this.storeService.CurrentContactChanged += OnCurrentContactChanged;
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
                        var cChat = storeService.Get(CommonKeys.CurrentChat.ToString()) as Chat;
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
            if (!storeService.HasKey(CommonKeys.CurrentChat.ToString())) return; //data already in store for when i want it
            var currentChat = storeService.Get(CommonKeys.CurrentChat.ToString()) as Chat;
            var currentChatId = currentChat.Id;
            if (eventArgs.ChatId == currentChatId)
            {
                if (Messages.Count == 0) //new fix();
                    Messages = new ObservableCollection<Massage>(currentChat.Messages);
                else
                {
                    Messages.Add(eventArgs.Massage);
                }
            }
        }

        private async void AddMessageHandler()
        {
            if (!Message.IsEmptyNullOrWhiteSpace())
            {
                var isSuccesfull = await messagesService.CallServerToAddMessage(Message);
                if (isSuccesfull)
                    Message = "";
            }
        }
    }
}
