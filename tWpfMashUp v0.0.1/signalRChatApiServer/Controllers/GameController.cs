using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using signalRChatApiServer.Hubs;
using signalRChatApiServer.Models;
using signalRChatApiServer.Repositories.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace signalRChatApiServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private IChatsReposatory chatReposatory;
        private IUsersReposatory usersReposatory;
        private IHubContext<ChatHub> chathub;
        public GameController(IChatsReposatory reposatory, IHubContext<ChatHub> chathub, IUsersReposatory usersReposatory)
        {
            this.usersReposatory = usersReposatory;
            this.chatReposatory = reposatory;
            this.chathub = chathub;
        }
        [HttpPost]
        public async Task Post(ActionUpdateModel obj)
        {
            var chat = chatReposatory.GetChat(obj.ChatId);
            obj.InverseRows();
            //send to other user
            var user = chat.Users.First(u => u.Id != obj.UserId);
            await chathub.Clients.Client(user.HubConnectionString).SendAsync("OpponentPlayed", obj);
        }
        [HttpGet]
        public async Task Get(int userId)
        {
            User user = usersReposatory.GetUser(userId);
           await chathub.Clients.Client(user.HubConnectionString).SendAsync("PlayerFinnishedPlay");
        }

       
    }
}
