using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using signalRChatApiServer.Hubs;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using signalRChatApiServer.Models;
using signalRChatApiServer.Repositories.Infra;

namespace signalRChatApiServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GameController : Controller
    {
        private IChatsReposatory reposatory;
        private IHubContext<ChatHub> chathub;

        public GameController(IHubContext<ChatHub> chathub, IChatsReposatory reposatory)
        {
            this.reposatory = reposatory;
            this.chathub = chathub;
        }

        [HttpPut]
        public void Put(Chat chat)
        {
            foreach (var user in chat.Users)
            {
                chathub.Clients.Client(user.HubConnectionString).SendAsync("GameInvite", chat);
            }
        }
        [HttpGet]
        public async void Get(int chatId, bool accepted)
        {
            //turn if accepted
            var chat = reposatory.GetChat(chatId);
            if (string.IsNullOrEmpty(chat.GameAproval)) chat.GameAproval = "";
            if (accepted)
            {
                //turn on one bit
                chat.GameAproval += "/";
                reposatory.UpdateChat(chat);
                //if two bits are on
                if (chat.GameAproval.Length > 1)
                {
                    //  push both game start
                    foreach (var user in chat.Users)
                    {
                        //  the two users switches views with the current chat
                        chathub.Clients.Client(user.HubConnectionString).SendAsync("GameStarting", chat);
                    }
                    //reset to enable another invites;
                    chat.GameAproval = "";
                    reposatory.UpdateChat(chat);
                }
            }
            //if deny push both on deny
            else
            {
                chat.GameAproval = "";
                reposatory.UpdateChat(chat);
                //  game cancel popup,
                //  action chain discontinuse,
                //  turn of both bits

                foreach (var user in chat.Users)
                {
                    chathub.Clients.Client(user.HubConnectionString).SendAsync("GameDenied", chat);
                }
            }
        }

    }
}
