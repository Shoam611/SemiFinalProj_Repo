using Microsoft.AspNetCore.Mvc;
using signalRChatApiServer.Repositories;

namespace signalRChatApiServer.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class ChatsController : Controller
    {
        IRepository repository;
        public ChatsController(IRepository repository) => this.repository = repository;
        [HttpGet]
        public void Get(int id)
        {
            var testobj = repository.GetUserChatsById(id);
        }
    }
}
