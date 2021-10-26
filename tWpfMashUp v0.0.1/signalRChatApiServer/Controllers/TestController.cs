using Microsoft.AspNetCore.Mvc;
using signalRChatApiServer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace signalRChatApiServer.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class TestController : Controller
    {
        IRepository repository;
        public TestController(IRepository repository) => this.repository = repository;
       [HttpGet]
        public void Get(int id)
        {
            var testobj = repository.GetUserChatsById(id);
        }
    }
}
