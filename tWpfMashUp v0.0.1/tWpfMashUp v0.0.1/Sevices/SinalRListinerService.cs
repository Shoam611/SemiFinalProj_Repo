using System;
using System.Diagnostics;
using System.Threading.Tasks;
using tWpfMashUp_v0._0._1.MVVM.Models;
using Microsoft.AspNetCore.SignalR.Client;

namespace tWpfMashUp_v0._0._1.Sevices
{
    public class SinalRListinerService
    {
        private readonly StoreService store;
        private readonly HubConnection connection;

        public SinalRListinerService(StoreService store)
        {
            this.store = store;
            connection = new HubConnectionBuilder().WithUrl("http://localhost:14795/ChatHub").Build();
            connection.Closed += async (err) => { await Task.Delay(3000); await connection.StartAsync(); };
            StartConnectionAsync();
        }

        private async void StartConnectionAsync()
        {

            connection.On<string>("Connected", OnConnected);
            connection.On<Chat>("OnChatAdd", ChatAddHandler);
            connection.On<Message, string>("MassageSent", OnNewMessege);
            try
            {
                if (connection.State != HubConnectionState.Connected)
                    await connection.StartAsync();
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }
        }

        private void OnNewMessege(object Message, string arg2)
        {
            
        }

        private void ChatAddHandler(Chat obj)
        {
            
        }

        private void OnConnected(string obj)
        {
            store.Add(CommonKeys.HubConnectionString.ToString(), obj);
        }
    }
}
