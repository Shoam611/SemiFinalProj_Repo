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
        public Chat Get(int userId, int toUserId)
        { 
            return repository.CreateChatWithUser(userId, toUserId);
        }

        [HttpPost]
        public void Post(Chat chat) => repository.AddChat(chat);

        [HttpPut]
        public void Put(Chat chat) => repository.UpdateChat(chat);
    }
}
