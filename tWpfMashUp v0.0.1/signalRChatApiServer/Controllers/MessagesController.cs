using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using signalRChatApiServer.Hubs;
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
        private IHubContext<ChatHub> chathub;
        public MessagesController(IRepository repository, IHubContext<ChatHub> chathub)
        {
            this.repository = repository;
            this.chathub = chathub;
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
