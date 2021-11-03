using System.Linq;
using Microsoft.AspNetCore.Mvc;
using signalRChatApiServer.Hubs;
using System.Collections.Generic;
using Microsoft.AspNetCore.SignalR;
using signalRChatApiServer.Models;
using signalRChatApiServer.Repositories;
using signalRChatApiServer.Repositories.Infra;

namespace signalRChatApiServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MessagesController : Controller
    {
        private readonly IHubContext<ChatHub> chathub;
        readonly IMassegesReposatory repository;     
        public MessagesController(IMassegesReposatory repository, IHubContext<ChatHub> chathub)
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
            var chat = repository.GetChat(message.ChatId);

            var userConnection = chat.Users.First().HubConnectionString;
            chathub.Clients.Client(userConnection).SendAsync("MassageRecived", message, chat.Id);
        }

    }
}
