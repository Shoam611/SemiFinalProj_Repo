using Microsoft.AspNetCore.Mvc;
using signalRChatApiServer.Models;
using signalRChatApiServer.Repositories;
using System.Collections.Generic;

namespace signalRChatApiServer.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class ChatController : Controller
    {
        IRepository repository;
        public ChatController(IRepository repository) => this.repository = repository;
       
        [HttpGet]
        public IEnumerable<Chat> Get(int userId)
        {
            return repository.GetUserChatsById(userId);
        }

        [HttpPost]
        public void Post(Chat chat) => repository.AddChat(chat);
    }
}
