using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using signalRChatApiServer.Hubs;
using signalRChatApiServer.Models;
using signalRChatApiServer.Repositories;
using signalRChatApiServer.Repositories.Infra;
using System.Linq;
using System.Threading.Tasks;

namespace signalRChatApiServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ChatController : Controller
    {
        private readonly IHubContext<ChatHub> chathub;
        private readonly IChatsReposatory repository;
        public ChatController(IChatsReposatory repository, IHubContext<ChatHub> chathub)
        {
            this.chathub = chathub;
            this.repository = repository;
        }

        [HttpGet]
        public Chat Get(int user1Id, int user2Id)
        {
            repository.IsChatExist(user1Id, user2Id, out Chat c);
            var contact = c.Users.First(c => c.Id == user2Id);
            chathub.Clients.Client(contact.HubConnectionString).SendAsync("ChatCreated", c);
            return c;
        }

        [HttpPost]
        public void Post(Chat chat) => repository.AddChat(chat);

        [HttpPut]
        public void Put(Chat chat) => repository.UpdateChat(chat);
    }
}
