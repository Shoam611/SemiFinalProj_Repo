using System.Linq;
using Microsoft.AspNetCore.Mvc;
using signalRChatApiServer.Hubs;
using System.Collections.Generic;
using signalRChatApiServer.Models;
using Microsoft.AspNetCore.SignalR;
using signalRChatApiServer.Repositories;

namespace signalRChatApiServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MessagesController : Controller
    {
        private IHubContext<ChatHub> chathub;
        readonly IRepository repository;
        public MessagesController(IRepository repository, IHubContext<ChatHub> chatHub)
        {
            this.chathub = chatHub;
            this.repository = repository;
        }

        [HttpGet]
        public List<Message> Get(int chatId)
        {
            return repository.GetMessages(chatId).ToList() ?? new List<Message>();
        }

        //Post
        [HttpPost]
        public void Post(Message message)
        {            
            repository.AddMessage(message);
            Chat c = repository.GetChat(message.ChatId);
            foreach(var u in c.Users)
            {
                chathub.Clients.Client(u.HubConnectionString).SendAsync("MassageRecived", c.Id);
            }
        }
    }
}
