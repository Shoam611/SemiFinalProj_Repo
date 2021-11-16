using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using signalRChatApiServer.Hubs;
using signalRChatApiServer.Models;
using signalRChatApiServer.Repositories.Infra;
using System;

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
                    var rnd = new Random().Next(0, 2);
                    var temp = 0;
                    foreach (var user in chat.Users)
                    {
                        chathub.Clients.Client(user.HubConnectionString).SendAsync("GameStarting", chat.Id,temp==rnd);
                        temp++;
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
                    try
                    {
                    chathub.Clients.Client(user.HubConnectionString).SendAsync("GameDenied", chat.Id);
                    }catch { }
                }
            }
        }
    }
}
