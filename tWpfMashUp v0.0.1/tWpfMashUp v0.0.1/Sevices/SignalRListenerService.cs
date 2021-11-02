using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using tWpfMashUp_v0._0._1.MVVM.Models;

namespace tWpfMashUp_v0._0._1.Sevices
{
    public class SignalRListenerService
    {
        private readonly StoreService store;
        private readonly HubConnection connection;
        private readonly MessagesService messagesService;

        public event EventHandler MassageRecived;
        public event EventHandler ContactLogged;

        public SignalRListenerService(StoreService store, MessagesService messagesService)
        {
            this.store = store;
            this.messagesService = messagesService;
            connection = new HubConnectionBuilder().WithUrl("http://localhost:14795/ChatHub").Build();
            connection.Closed += async (err) => { await Task.Delay(3000); await connection.StartAsync(); };
            StartConnectionAsync();
        }
        private async void StartConnectionAsync()
        {
            connection.On<string>("Connected", OnConnected);
            connection.On<int>("MassageRecived", OnMassageRecived);
            connection.On<User>("ContactLoggedIn", OnContactLogged);
            connection.On<User>("ContactLoggedOut", OnContactLoggedOut);

            try
            {
                if (connection.State != HubConnectionState.Connected)
                    await connection.StartAsync();
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }
        }

        private void OnConnected(string hubConnectionString) => store.Add(CommonKeys.HubConnectionString.ToString(), hubConnectionString);

        private void OnMassageRecived(Massage msg, int chatId) => MassageRecived?.Invoke(this, new MessageRecivedEventArgs { Massage = msg, ChatID = chatId });

        private void OnContactLogged(User newOnlineUser)
        {
            if (store.Get(CommonKeys.Contacts.ToString()) is not List<User> contacts)
                contacts = new List<User>();
            contacts.Add(newOnlineUser);
            store.Add(CommonKeys.Contacts.ToString(), contacts); 
                                 //sender => who called;                      args => all the arguments;
            ContactLogged?.Invoke(this, new ContactLoggedEventArgs { User = newOnlineUser, IsLoggedIn = true });
        }

        private void OnContactLoggedOut(User disconnectedUser)
        {
            //var contacts = store.Get(CommonKeys.Contacts.ToString()) as List<User>;
            //if (contacts == null) contacts = new List<User>();
            //contacts.Remove(disconnectedUser);
            //store.Add(CommonKeys.Contacts.ToString(), contacts);
            ContactLogged?.Invoke(this, new ContactLoggedEventArgs { User = disconnectedUser, IsLoggedIn = false });
        }

        private async void OnMassageRecived(int chatId)
        {
            var newMessages = await messagesService.GetChatMassages(chatId);
            (store.Get(CommonKeys.CurrentChat.ToString()) as Chat).Messages = newMessages;
        }
    }
}
