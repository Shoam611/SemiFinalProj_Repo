using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using signalRChatApiServer.Hubs;
using signalRChatApiServer.Models;
using signalRChatApiServer.Repositories.Infra;
using System.Collections.Generic;

namespace signalRChatApiServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : Controller
    {
        private IHubContext<ChatHub> chathub;
        private IUsersRepository repository;

        public UsersController(IUsersRepository repository, IHubContext<ChatHub> chatHub)
        {
            chathub = chatHub;
            this.repository = repository;
        }

        [HttpGet]
        public List<User> Get()
        {
            return repository.GetAllUsers();
        }

        //update user-> status and h.c.string
        [HttpPut]
        public void Put(User user)
        {
            if (user.IsConnected == Status.Offline)
                chathub.Clients.AllExcept(user.HubConnectionString).SendAsync("ContactLoggedOut", user);
            repository.UpdateUser(user);
        }
    }
}
