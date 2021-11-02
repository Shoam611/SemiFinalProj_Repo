using System.Linq;
using Microsoft.AspNetCore.Mvc;
using signalRChatApiServer.Hubs;
using System.Collections.Generic;
using Microsoft.AspNetCore.SignalR;
using signalRChatApiServer.Models;
using signalRChatApiServer.Repositories;

namespace signalRChatApiServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MessagesController : Controller
    {
        private readonly IHubContext<ChatHub> chathub;
        readonly IRepository repository;     
        public MessagesController(IRepository repository, IHubContext<ChatHub> chathub)
        {
            this.chathub = chathub;
            this.repository = repository;        
        }

        [HttpGet]
        public List<Message> Get(int chatId)
        {
            return repository.GetMessages(chatId).ToList() ?? new List<Message>();
        }

        //Put
        [HttpPost]
        public void Post(Message message)
        {
            repository.AddMessage(message);

            var chatId = repository.GetChatByMessage(message.Id).Id;
            var userConnection = new List<User>(repository.GetChatByMessage(message.Id).Users).First().HubConnectionString;
            chathub.Clients.Client(userConnection).SendAsync("MassageRecived", message, chatId);
        }

    }
}
