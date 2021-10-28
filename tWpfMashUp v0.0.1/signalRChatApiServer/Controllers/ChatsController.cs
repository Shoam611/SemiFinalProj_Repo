using Microsoft.AspNetCore.Mvc;
using signalRChatApiServer.Models;
using signalRChatApiServer.Repositories;

namespace signalRChatApiServer.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class ChatsController : Controller
    {
        IRepository repository;
        public ChatsController(IRepository repository) => this.repository = repository;

        //[HttpGet]
        //public void Get(int id)
        //{
        //    var testobj = repository.GetUserChatsById(id);
        //}
        [HttpGet]
        public void Get(int userA, int userB)
        {
            var testobj = repository.GetChat(userA, userB);
        }

        [HttpPost]
        public void Post(Chat chat) => repository.AddChat(chat);
    }
}
