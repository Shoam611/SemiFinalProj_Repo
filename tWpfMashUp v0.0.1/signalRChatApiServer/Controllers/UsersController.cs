using Microsoft.AspNetCore.Mvc;
using signalRChatApiServer.Hubs;
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
        public User Get(string username, string password)
        {
            var isauth = repository.Authenticate(username, password);
            if (isauth != null)
            {
                isauth.IsConnected = Status.Online;
                repository.UpdateUser(isauth);
                chathub.Clients.AllExcept(isauth.HubConnectionString).SendAsync("ContactLoggedIn", isauth);
            }
            return isauth;
        }

        [HttpPost]
        public bool Post(User newUser)
        {
            try
            {                
                repository.AddUser(newUser);
                return true;
            }
            catch { return false; }
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
