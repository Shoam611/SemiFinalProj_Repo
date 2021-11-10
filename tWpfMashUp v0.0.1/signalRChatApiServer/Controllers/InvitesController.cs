﻿using Microsoft.AspNetCore.Mvc;
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
            if (accepted)
            {
                chat.InviteStatus = (InviteStatus)((int)chat.InviteStatus + 1);
                reposatory.UpdateChat(chat);

                if (chat.InviteStatus == InviteStatus.Accepted)
                {
                    foreach (var user in chat.Users)
                    {
                        chathub.Clients.Client(user.HubConnectionString).SendAsync("GameStarting", chat.Id);
                    }
                    chat.InviteStatus = InviteStatus.Empty;
                    reposatory.UpdateChat(chat);
                }
            }
            //if deny push both on deny
            else
            {
                chat.InviteStatus = InviteStatus.Empty;
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
