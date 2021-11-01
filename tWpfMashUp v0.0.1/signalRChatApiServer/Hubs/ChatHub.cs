using Microsoft.AspNetCore.SignalR;
using signalRChatApiServer.Models;
using System;
using System.Threading.Tasks;

namespace signalRChatApiServer.Hubs
{
    public class ChatHub : Hub
    {       

        public override Task OnConnectedAsync()
        {
            Clients.Caller.SendAsync("Connected", Context.ConnectionId);
                    
            return base.OnConnectedAsync();
        }

        public void InformClientLoggedIn(User user) => Clients.AllExcept(user.HubConnectionString).SendAsync("ContactLoggedIn", user);
        public void InformClientLoggedOut(User user) => Clients.AllExcept(user.HubConnectionString).SendAsync("ContactLoggedOut", user);
        public void InformClientMassageRecived(Message message, int chatId, string hubConnectionString) => Clients.Client(hubConnectionString).SendAsync("MassageRecived", message);

    }
}
