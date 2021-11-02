using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using signalRChatApiServer.Hubs;
using signalRChatApiServer.Models;
using signalRChatApiServer.Repositories;

namespace signalRChatApiServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ChatController : Controller
    {
        private readonly IHubContext<ChatHub> chathub;

        IRepository repository;
        public ChatController(IRepository repository, IHubContext<ChatHub> chathub)
        {
            this.chathub = chathub;
            this.repository = repository;
        }

        [HttpGet]
        public Chat Get(int user1Id, int user2Id)
        {
            repository.IsChatExist(user1Id, user2Id, out Chat c);
            return c;
        }

        [HttpPost]
        public void Post(Chat chat) => repository.AddChat(chat);

        [HttpPut]
        public void Put(Chat chat) => repository.UpdateChat(chat);
    }
}
