using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace signalRChatApiServer.Hubs
{
    public class ChatHub : Hub
    {
        public async Task MessageAll(string sender, string message)
            => await Clients.All.SendAsync("NewMessage", sender, message);

        //public override 

        public override Task OnConnectedAsync()
        {
            Clients.Caller.SendAsync("Connected", Context.ConnectionId);
            return base.OnConnectedAsync();
        }
    }
}
