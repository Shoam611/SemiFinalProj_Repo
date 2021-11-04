using System;
using System.Linq;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;
using tWpfMashUp_v0._0._1.MVVM.Models;
using Microsoft.AspNetCore.SignalR.Client;
using System.Windows;

namespace tWpfMashUp_v0._0._1.Sevices
{
    public delegate void MessageRecivedEventHandler(object sender, MessageRecivedEventArgs eventArgs);
    public class SignalRListenerService
    {
        private readonly StoreService store;
        private readonly MessagesService messagesService;
        private readonly HubConnection connection;

        public event MessageRecivedEventHandler MessageRecived;
        public event EventHandler ContactLogged;

        public SignalRListenerService(StoreService store, MessagesService messagesService)
        {

            this.store = store;
            this.messagesService = messagesService;
            connection = new HubConnectionBuilder().WithUrl("http://localhost:14795/ChatHub").Build();
            connection.Closed += async (err) => { await Task.Delay(2500); await connection.StartAsync(); };
            StartConnectionAsync();
        }


        public async void StartConnectionAsync()
        {
            connection.On<string>("Connected", OnConnected);
            connection.On<Massage>("MassageRecived", OnMassageRecived);
            connection.On<User>("ContactLoggedIn", OnContactLoggedIn);
            connection.On<User>("ContactLoggedOut", OnContactLoggedOut);
            connection.On<Chat>("ChatCreated", OnChatCreated);
            try
            {
                if (connection.State != HubConnectionState.Connected) await connection.StartAsync();
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }
        }

        private void OnChatCreated(Chat obj)
        {
            if (store.HasKey(CommonKeys.Chats.ToString()))
            {
                var chats = store.Get(CommonKeys.Chats.ToString()) as List<Chat>;
                if (chats.FirstOrDefault(c => c.Id == obj.Id) == null)
                    chats.Add(obj);
            }
            else
            {
                store.Add(CommonKeys.Chats.ToString(), new List<Chat> { obj });
            }
        }
        private void OnConnected(string hubConnectionString)
        {
            store.Add(CommonKeys.HubConnectionString.ToString(), hubConnectionString);
        }
           
        private void OnMassageRecived(Massage msg)
        {
            var chats = store.Get(CommonKeys.Chats.ToString()) as List<Chat>;
            var chat = chats.FirstOrDefault(c => c.Id == msg.ChatId);
            if (chat.Messages == null) chat.Messages = new List<Massage>();
            chat.Messages.Add(msg);
            MessageRecived?.Invoke(this, new MessageRecivedEventArgs { ChatId = chat.Id, Massage = msg });                  
        }

        private void OnContactLoggedIn(User newOnlineUser)
        {
            List<User> contacts;
            if (!store.HasKey(CommonKeys.Contacts.ToString()))
                contacts = new List<User>();
            else contacts = store.Get(CommonKeys.Contacts.ToString()) as List<User>;
            if (! contacts.Where(u => u.Id == newOnlineUser.Id).Any())
            {
            contacts.Add(newOnlineUser);          
            }
            store.Add(CommonKeys.Contacts.ToString(), contacts);          
            ContactLogged?.Invoke(this, new ContactLoggedEventArgs { User = newOnlineUser, IsLoggedIn = true });
        }

        private void OnContactLoggedOut(User disconnectedUser)
        {
            ContactLogged?.Invoke(this, new ContactLoggedEventArgs { User = disconnectedUser, IsLoggedIn = false });
            if (store.HasKey(CommonKeys.Chats.ToString()))
            {
                var chats =store.Get(CommonKeys.Chats.ToString()) as List<Chat>;
                var chat = chats.FirstOrDefault(c => c.Users.FirstOrDefault(u => u.Id == disconnectedUser.Id)!=null);
                if (chat != null)
                {
                    chats.Remove(chat);
                }
            }
        }

    }
}
