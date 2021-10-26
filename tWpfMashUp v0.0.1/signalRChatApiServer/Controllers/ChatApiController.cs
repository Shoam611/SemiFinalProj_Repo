using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using signalRChatApiServer.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace signalRChatApiServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ChatApiController : ControllerBase
    {
        IHubContext<ChatHub> hbcntx;
        public ChatApiController(IHubContext<ChatHub> hbcntx)
        {
            this.hbcntx = hbcntx;
        }
        

        [HttpGet]
        public string Get()
        {
            return "In Get Action";
        }
        [HttpPost]
        public void Post(string value) => hbcntx.Clients.All.SendAsync("Posted", value);
        /* http://localhost:14795/ChatApi?value=TestValue */
    }
}
