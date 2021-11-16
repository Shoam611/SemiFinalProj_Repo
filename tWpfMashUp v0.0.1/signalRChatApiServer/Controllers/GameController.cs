using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Castle.Core;
using Microsoft.AspNetCore.SignalR;
using signalRChatApiServer.Hubs;
using signalRChatApiServer.Models;
using signalRChatApiServer.Repositories.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace signalRChatApiServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private IChatsReposatory reposatory;
        private IHubContext<ChatHub> chathub;
        public GameController(IChatsReposatory reposatory, IHubContext<ChatHub> chathub)
        {
            this.reposatory = reposatory;
            this.chathub = chathub;
        }
        [HttpPost]
        public async Task Get(ActionUpdateModel obj)
        {
            var chat = reposatory.GetChat(obj.ChatId);
            obj.InverseRows();
            //send to other user
            var user = chat.Users.First(u => u.Id != obj.UserId);
            await chathub.Clients.Client(user.HubConnectionString).SendAsync("OpponentPlayed", obj);
        }

       
    }
}
