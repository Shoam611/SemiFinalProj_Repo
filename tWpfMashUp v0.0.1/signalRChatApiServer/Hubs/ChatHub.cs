using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace signalRChatApiServer.Hubs
{
    public class ChatHub : Hub
    {
        public override Task OnConnectedAsync()
        {
            Clients.Caller.SendAsync("Connected", Context.ConnectionId);

            return base.OnConnectedAsync();
        }
    }
}
