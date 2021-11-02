using Microsoft.AspNetCore.Mvc;
using signalRChatApiServer.Models;
using signalRChatApiServer.Repositories;

namespace signalRChatApiServer.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class ChatController : Controller
    {
        IRepository repository;
        public ChatController(IRepository repository) => this.repository = repository;
       
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
