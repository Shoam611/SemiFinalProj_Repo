using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using signalRChatApiServer.Hubs;
using signalRChatApiServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace signalRChatApiServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GameController : Controller
    {
        private IHubContext<ChatHub> chathub;

        public GameController(IHubContext<ChatHub> chathub)
        {
            this.chathub = chathub;
        }

        [HttpPut]
        public void Put(User user)
        {
            chathub.Clients.Client(user.HubConnectionString).SendAsync("GameInvite", user);
        }

    }
}
