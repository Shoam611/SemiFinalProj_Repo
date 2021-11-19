using System;
using System.Windows.Controls;
using tWpfMashUp_v0._0._1.Core;
using tWpfMashUp_v0._0._1.Sevices;
using System.Collections.ObjectModel;
using tWpfMashUp_v0._0._1.Extentions;
using tWpfMashUp_v0._0._1.MVVM.Models;
using System.Windows;

namespace tWpfMashUp_v0._0._1.MVVM.ViewModels
{
    public class ChatThreadViewModel : ObservableObject
    {
        private readonly MessagesService messagesService;
        private readonly StoreService storeService;
        private readonly SignalRListenerService listenerService;
        // private readonly ChatsService chatService;

        private string currentContact;
        public string CurrentContact { get => currentContact; set { currentContact = value; onProppertyChange(); } }

        private ObservableCollection<Message> messages;
        public ObservableCollection<Message> Messages { get => messages; set { messages = value; onProppertyChange(); } }

        private string message;
        public string Message { get => message; set { message = value; onProppertyChange(); } }

        public RelayCommand AddMessageCommand { get; set; }
     
        public ChatThreadViewModel(MessagesService messagesService, ChatsService chatService, StoreService storeService, SignalRListenerService listenerService)
        {
            CurrentContact = "";
            // this.chatService = chatService;
            this.storeService = storeService;
            this.listenerService = listenerService;
            this.messagesService = messagesService;
            Messages = new ObservableCollection<Message>();
            AddMessageCommand = new RelayCommand((o) => AddMessageHandler());
        
            this.storeService.CurrentContactChanged += OnCurrentContactChanged;
            this.listenerService.MessageRecived += OnMessageRecived;
            this.listenerService.ChatForUserRecived += OnCurrentContactChanged;
            this.listenerService.GameStarting += (s, e) => OnGameStarting();
            this.listenerService.ContactLogged += (s, e) => { if (!e.IsLoggedIn && e.User.UserName == CurrentContact) { Messages.Clear(); CurrentContact = ""; } };
        }

        private void OnGameStarting()
        {
            var cChat = storeService.Get(CommonKeys.CurrentChat.ToString()) as Chat;
            CurrentContact = (storeService.Get(CommonKeys.WithUser.ToString()) as User).UserName;
            Message = $"Good Luck {CurrentContact}!";
            Messages = new ObservableCollection<Message>(cChat.Messages);
        }

        private void OnCurrentContactChanged(object sender, EventArgs e)
        {
            try
            {
                var args = e as ChatRecivedEventArgs;
                CurrentContact = args.ContactName; ;
                if (args.NewChat != null)
                {
                    if (args.NewChat.Messages != null)
                        Messages = new ObservableCollection<Message>(args.NewChat.Messages);
                    else Messages.Clear();
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
                    Messages = new ObservableCollection<Message>(currentChat.Messages);
                else Messages.Add(eventArgs.Massage);
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
