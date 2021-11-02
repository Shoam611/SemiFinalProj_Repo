using System;
using System.Diagnostics;
using System.Threading.Tasks;
using tWpfMashUp_v0._0._1.MVVM.Models;
using Microsoft.AspNetCore.SignalR.Client;
using System.Collections.Generic;

namespace tWpfMashUp_v0._0._1.Sevices
{
    public class SignalRListinerService
    {
        private readonly StoreService store;
        private readonly MessagesService messagesService;
        private readonly HubConnection connection;
        //public event EventHandler MassageRecived;
        public event EventHandler ContactLogged;

        public SignalRListinerService(StoreService store ,MessagesService messagesService,AuthenticationService authentication)
        {
            this.messagesService = messagesService;
            this.store = store;
            connection = new HubConnectionBuilder().WithUrl("http://localhost:14795/ChatHub").Build();
            connection.Closed += async (err) => { await Task.Delay(2500); await connection.StartAsync(); };
            //authentication.LoggingIn += (s, e) => StartServerListening();
            StartConnectionAsync();
        }


        private async void StartConnectionAsync()
        {
            connection.On<string>("Connected", OnConnected);
            connection.On<int>("MassageRecived", OnMassageRecived);
            connection.On<User>("ContactLoggedIn", OnContactLoggedIn);
            connection.On<User>("ContactLoggedOut", OnContactLoggedOut);
            try
            {
                if (connection.State != HubConnectionState.Connected) await connection.StartAsync();
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }
        }

        private void StartServerListening()
        {
            connection.On<int>("MassageRecived", OnMassageRecived);
            connection.On<User>("ContactLoggedIn", OnContactLoggedIn);
            connection.On<User>("ContactLoggedOut", OnContactLoggedOut);
        }

        private void OnConnected(string hubConnectionString) => store.Add(CommonKeys.HubConnectionString.ToString(), hubConnectionString);

        private async void OnMassageRecived(int chatId)
        {
            var newMsgs = await messagesService.GetChatMassages(chatId);
            (store.Get(CommonKeys.CurrentChat.ToString()) as Chat).Messages = newMsgs;
            //inform Massage Recived
        }

        private void OnContactLoggedIn(User newOnlineUser)
        {
            var contacts = store.Get(CommonKeys.Contacts.ToString()) as List<User>;
            if (contacts == null) contacts = new List<User>();
            contacts.Add(newOnlineUser);
            store.Add(CommonKeys.Contacts.ToString(), contacts);//sender => who called; args => all the arguments
            ContactLogged?.Invoke(this, new ContactLoggedEventArgs {User=newOnlineUser,IsLoggedIn=true});
        }

        private void OnContactLoggedOut(User disconnectedUser)
        {
            //var contacts = store.Get(CommonKeys.Contacts.ToString()) as List<User>;
            //if (contacts == null) contacts = new List<User>();
            //contacts.Remove(disconnectedUser);
            //store.Add(CommonKeys.Contacts.ToString(), contacts);
            ContactLogged?.Invoke(this, new ContactLoggedEventArgs { User = disconnectedUser, IsLoggedIn = false });
        }

    }
}
