using Microsoft.AspNetCore.Mvc;
using signalRChatApiServer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using signalRChatApiServer.Models;
namespace signalRChatApiServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : Controller
    {
        IRepository repository;
        public UsersController(IRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        public User Get(string username, string password) => repository.Authenticate(username, password) ?? null;

        [HttpPost]
        public bool Post(Dictionary<string, string> user)
        {
            try
            {
                var newuser = new User {UserName = user["UserName"],Password=user["Password"] };
                repository.AddUser(newuser);return true;
            }
            catch { return false; }
        }

        [HttpPut]
        public void Put(User user) => repository.UpdateUser(user);
    }
}
