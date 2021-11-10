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
        //private static string gameAproval;

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
            var chat = reposatory.GetChat(chatId);
            if (string.IsNullOrEmpty(chat.GameAproval)) chat.GameAproval = "";
            if (accepted)
            {
                chat.GameAproval += "/";
                reposatory.UpdateChat(chat);

                if (chat.GameAproval.Length > 1)
                {
                    //foreach (var user in chat.Users)
                    //{
                    //    user.Chats = null; user.ChatUsers = null;
                    //}
                    //chat.ChatUsers = null;
                    //chat.GameAproval = null;

                    foreach (var user in chat.Users)
                    {
                        chathub.Clients.Client(user.HubConnectionString).SendAsync("GameStarting", chat.Id);
                    }
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
