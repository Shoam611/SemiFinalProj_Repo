using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using signalRChatApiServer.Hubs;
using signalRChatApiServer.Models;
using signalRChatApiServer.Repositories;
using signalRChatApiServer.Repositories.Infra;
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
        private readonly IHubContext<ChatHub> chathub;
        private readonly IUsersReposatory repository;

        public AuthenticationController(IUsersReposatory repository, IHubContext<ChatHub> chatHub)
        {
            this.chathub = chatHub;
            this.repository = repository;
        }
      //for login
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
        //for sighn up
        [HttpPost]
        public bool Post(User newUser)
        {
            try
            {
                var isExist = repository.IsUserExist(newUser.UserName);
                if (!isExist)
                {
                    repository.AddUser(newUser);
                    return true;
                }
                return false;
            }
            catch { return false; }
        }
    }
}
