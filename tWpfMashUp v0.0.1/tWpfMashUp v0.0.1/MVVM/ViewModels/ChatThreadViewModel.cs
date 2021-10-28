﻿using System.Collections.Generic;
using tWpfMashUp_v0._0._1.Core;
using tWpfMashUp_v0._0._1.MVVM.Models;
using tWpfMashUp_v0._0._1.Sevices;

namespace tWpfMashUp_v0._0._1.MVVM.ViewModels
{
    public class ChatThreadViewModel : ObservableObject
    {
        MessagesService messagesService;
        public List<Message> Messages { get; set; }
        public RelayCommand AddMessageCommand { get; set; }
        public string Message { get; set; }
        public ChatThreadViewModel(MessagesService messagesService) //
        {
            this.messagesService = messagesService;
            AddMessageCommand = new RelayCommand((o) => AddMessageHandler());
        }

        private async void AddMessageHandler()
        {
            await messagesService.CallServerToAddMessage(Message);
        }
    }
}
