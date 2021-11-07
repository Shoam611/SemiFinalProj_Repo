using System.Linq;
using Microsoft.AspNetCore.Mvc;
using signalRChatApiServer.Hubs;
using System.Collections.Generic;
using Microsoft.AspNetCore.SignalR;
using signalRChatApiServer.Models;
using signalRChatApiServer.Repositories;
using signalRChatApiServer.Repositories.Infra;
using System.Threading.Tasks;

namespace signalRChatApiServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MessagesController : Controller
    {
        private readonly IHubContext<ChatHub> chathub;
        readonly IMassegesReposatory repository;     
        readonly IChatsReposatory chatrepository;     
        public MessagesController(IMassegesReposatory repository, IHubContext<ChatHub> chathub, IChatsReposatory chatrepository)
        {
            this.chathub = chathub;
            this.chatrepository = chatrepository;
            this.repository = repository;        
        }

        [HttpGet]
        public List<Message> Get(int chatId)
        {
            return repository.GetMessages(chatId).ToList() ?? new List<Message>();
        }

        //Put
        [HttpPost]
        public async Task Post(Message message)
        {
            repository.AddMessage(message);            
            var chat = chatrepository.GetChat(message.ChatId);

            foreach (var item in chat.Users)
            {   
            await chathub.Clients.Client(item.HubConnectionString).SendAsync("MassageRecived", message);
            }
        }

    }
}
