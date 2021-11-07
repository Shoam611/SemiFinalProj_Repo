﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using signalRChatApiServer.Hubs;
using signalRChatApiServer.Models;
using signalRChatApiServer.Repositories;
using signalRChatApiServer.Repositories.Infra;
using System.Linq;

namespace signalRChatApiServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ChatController : Controller
    {
        private readonly IHubContext<ChatHub> chathub;

        IChatsReposatory repository;
        public ChatController(IChatsReposatory repository, IHubContext<ChatHub> chathub)
        {
            this.chathub = chathub;
            this.repository = repository;
        }

        [HttpGet]
        public void /*Chat*/ Get(int user1Id, int user2Id)
        {
            repository.IsChatExist(user1Id, user2Id, out Chat obj);
            foreach (var user in obj.Users)
            {
                user.Chats = null;//.Clear();
                user.ChatUsers = null; //.Clear();
            }
            obj.ChatUsers = null;
            foreach (var contact in obj.Users)
            {
                chathub.Clients.Client(contact.HubConnectionString).SendAsync("ChatCreated", obj);
            }
            //return c;
        }

        [HttpPost]
        public void Post(Chat chat) => repository.AddChat(chat);

        [HttpPut]
        public void Put(Chat chat) => repository.UpdateChat(chat);
    }
}
