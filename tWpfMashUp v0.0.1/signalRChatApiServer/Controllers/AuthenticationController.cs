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
    public class AuthenticationController : Controller
    {
        private IHubContext<ChatHub> chathub;
        private IRepository repository;

        public AuthenticationController(IRepository repository, IHubContext<ChatHub> chatHub)
        {
            this.chathub = chatHub;
            this.repository = repository;
        }
      
        [HttpGet]
        public User Get(string username, string password,string hubstring)
        {
            var isauth = repository.Authenticate(username, password);
            if (isauth != null)
            {
                isauth.HubConnectionString = hubstring;
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
    }
}
