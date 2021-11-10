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
    public class InvitesController : Controller
    {
        private IChatsReposatory reposatory;
        private IHubContext<ChatHub> chathub;
        private static string gameAproval;

        public InvitesController(IHubContext<ChatHub> chathub, IChatsReposatory reposatory)
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
        public void Get(int chatId, bool accepted)
        {
            //turn if accepted
            var chat = reposatory.GetChat(chatId);
            if (string.IsNullOrEmpty(gameAproval)) gameAproval = "";
            if (accepted)
            {
                //turn on one bit
                gameAproval += "/";
                //if two bits are on
                if (gameAproval.Length > 1)
                {
                    foreach (var user in chat.Users)
                    {
                        user.Chats = null; user.ChatUsers = null;
                    }
                    chat.ChatUsers = null;
                    //  push both game start
                    foreach (var user in chat.Users)
                    {
                        //  the two users switches views with the current chat
                        chathub.Groups.AddToGroupAsync(user.HubConnectionString, "Accepted");
                        //await chathub.Clients.Client(user.HubConnectionString).SendAsync("GameStarting", chat);
                    }
                    chathub.Clients.Group("Accepted").SendAsync("GameStarting", chat);
                    //reset to enable another invites;
                    gameAproval = "";
                }
            }
            //if deny push both on deny
            else
            {
                gameAproval = "";
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
