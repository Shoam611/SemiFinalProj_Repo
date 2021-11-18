using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using signalRChatApiServer.Hubs;
using signalRChatApiServer.Models;
using Microsoft.AspNetCore.SignalR;
using signalRChatApiServer.Repositories.Infra;

namespace signalRChatApiServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private IChatsRepository chatRepository;
        private IUsersRepository usersRepository;
        private IHubContext<ChatHub> chatHub;

        public GameController(IChatsRepository chatRepository, IHubContext<ChatHub> chatHub, IUsersRepository usersRepository)
        {
            this.usersRepository = usersRepository;
            this.chatRepository = chatRepository;
            this.chatHub = chatHub;
        }

        [HttpPost]
        public async Task Post(ActionUpdateModel obj)
        {
            var chat = chatRepository.GetChat(obj.ChatId);
            obj.InverseRows();
            //send to other user
            var user = chat.Users.First(u => u.Id != obj.UserId);
            await chatHub.Clients.Client(user.HubConnectionString).SendAsync("OpponentPlayed", obj);
        }

        [HttpGet]
        public async Task Get(int userId)
        {
            User user = usersRepository.GetUser(userId);
            await chatHub.Clients.Client(user.HubConnectionString).SendAsync("PlayerFinnishedPlay");
        }

        [HttpGet]
        [Route("Forfeit")]
        public async Task Get(string chatId)
        {
            Chat chat = chatRepository.GetChat(int.Parse(chatId));
            foreach (var user in chat.Users)
            {
                await chatHub.Clients.Client(user.HubConnectionString).SendAsync("GameEnded");
            }
        }
    }
}
