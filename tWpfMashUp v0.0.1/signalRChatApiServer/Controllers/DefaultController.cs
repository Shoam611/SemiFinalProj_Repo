using Microsoft.AspNetCore.Mvc;
using signalRChatApiServer.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace signalRChatApiServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DefaultController : ControllerBase
    {
        readonly IHubContext<ChatHub> hbcntx;
        public DefaultController(IHubContext<ChatHub> hbcntx) => this.hbcntx = hbcntx;

        [HttpGet]
        public string Get() => "In Default Get Action";

        [HttpPost]
        public void Post(string value) => hbcntx.Clients.All.SendAsync("Posted", value);
        
        /* http://localhost:14795/ChatApi?value=TestValue */
    }
}
