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
    public class UsersController : Controller
    {
        private IHubContext<ChatHub> chathub;
        IRepository repository;

        public UsersController(IRepository repository, IHubContext<ChatHub> chatHub)
        {
            chathub = chatHub;
            this.repository = repository;
        }
   
        [HttpGet]
        public List<User> Get()
        {
           return repository.GetAllUsers();
        }

        [HttpPut]
        public void Put(User user)
        {
            if (user.IsConnected == Status.Offline)
                chathub.Clients.AllExcept(user.HubConnectionString).SendAsync("ContactLoggedOut", user);
            repository.UpdateUser(user);
        }
    }
}
