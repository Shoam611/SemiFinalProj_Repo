using Microsoft.AspNetCore.Mvc;
using signalRChatApiServer.Hubs;
using System.Collections.Generic;
using signalRChatApiServer.Models;
using Microsoft.AspNetCore.SignalR;
using signalRChatApiServer.Repositories.Infra;
using System.Threading.Tasks;

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

        //[HttpGet]
        //[Route("Logout")]
        //public async Task Get()
        //{
        //    foreach (var user in chat.Users)
        //    {
        //        await chathub.Clients.Client(user.HubConnectionString).SendAsync("Loggingout");
        //    }
        //}

        //update user-> status and h.c.string
        [HttpPut]
        public void Put(User user)
        {
            if (user.Status == Status.Offline)
            {
                chathub.Clients.AllExcept(user.HubConnectionString).SendAsync("ContactLoggedOut", user);
                chathub.Clients.Client(user.HubConnectionString).SendAsync("LoggingOut", user);
            }
            repository.UpdateUser(user);
        }

    }
}
