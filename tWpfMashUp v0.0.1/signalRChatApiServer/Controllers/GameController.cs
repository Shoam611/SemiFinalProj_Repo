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
        [HttpGet]
        [Route("announcevictory")]
        public void AnnounceWinner(int userId,int chatId)
        {
            var chat = chatReposatory.GetChat(chatId);
            var user = chat.Users.First(u => u.Id != userId);
            chathub.Clients.Client(user.HubConnectionString).SendAsync("GameOver");
        }
       

        [HttpGet]
        [Route("Forfeit")]
        public async Task Get(string chatId)
        {
            Chat chat = chatReposatory.GetChat(int.Parse(chatId));
            foreach (var user in chat.Users)
            {
                await chathub.Clients.Client(user.HubConnectionString).SendAsync("GameEnded");
            }
        }
    }
}
