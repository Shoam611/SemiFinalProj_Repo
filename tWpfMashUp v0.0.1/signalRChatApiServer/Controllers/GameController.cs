using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using signalRChatApiServer.Hubs;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using signalRChatApiServer.Models;

namespace signalRChatApiServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GameController : Controller
    {
        private IHubContext<ChatHub> chathub;

        public GameController(IHubContext<ChatHub> chathub) => this.chathub = chathub;

        [HttpPut]
        public void Put(Chat chat)
        {
            foreach (var user in chat.Users)
            {
            chathub.Clients.Client(user.HubConnectionString).SendAsync("GameInvite", user);
            }
        }
        [HttpGet]
        public void Get(int userId, int chatId)
        {
            //turn if accepted turn on one bit
            //if two bits are on
            //  push both game start
            //  the two users switches views with the current chat
            //if deny push both on deny
            //  game cancel popup,
            //  action chain discontinuse,
            //  turn of both bits
        }

    }
}
