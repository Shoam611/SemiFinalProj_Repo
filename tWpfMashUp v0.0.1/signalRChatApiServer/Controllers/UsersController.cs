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
        public User Get(int id) => repository.GetUser(id);
        
        [HttpPost]
        public void Post(User user) => repository.AddUser(user);

        [HttpPut]
        public void Put(User user) => repository.UpdateUser(user);
    }
}
