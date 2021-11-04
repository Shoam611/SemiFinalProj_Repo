using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using tWpfMashUp_v0._0._1.MVVM.Models;

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
            if (!store.HasKey(CommonKeys.Chats.ToString()))
            {
                (store.Get(CommonKeys.Chats.ToString()) as List<Chat>).Add(obj);
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
            if (!store.HasKey(CommonKeys.Chats.ToString()))
            {
            }
            else
            {
            var chats = store.Get(CommonKeys.Chats.ToString()) as List<Chat>;
            var chat = chats.FirstOrDefault(c => c.Id == msg.ChatId);
            if (chat.Messages == null) chat.Messages = new List<Massage>();
            chat.Messages.Add(msg);
            MessageRecived?.Invoke(this, new MessageRecivedEventArgs { ChatID = chat.Id ,Massage =msg});
            }
            //if chat is current chat
                    //push massage
            //else
                //add massage to store 
                //mark user as has not read massage                
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
        }

    }
}
