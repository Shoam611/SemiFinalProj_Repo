using Microsoft.AspNetCore.Mvc;
using signalRChatApiServer.Models;
using signalRChatApiServer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace signalRChatApiServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MessagesController : Controller
    {
        readonly IRepository repository;
        public MessagesController(IRepository repository)
        {
            this.repository = repository;
        }

        //[HttpGet]
        // public List<Message> Get() => repository.GetAllMessages();

        [HttpGet]
        public List<Message> Get(int chatId) => repository.GetMessages(chatId) ?? new List<Message>();

        //Post
        [HttpPost]
        public void Post(Message message) => repository.AddMessage(message);
    }
}
