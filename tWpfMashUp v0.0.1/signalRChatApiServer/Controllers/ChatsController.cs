﻿using Microsoft.AspNetCore.Mvc;
using signalRChatApiServer.Models;
using signalRChatApiServer.Repositories;
using System.Collections.Generic;

namespace signalRChatApiServer.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class ChatController : Controller
    {
        IRepository repository;
        public ChatController(IRepository repository) => this.repository = repository;
       
        [HttpGet]
        public Chat Get(int userId,int toUser=1)
        {
            return repository.CreateChatWithRandomUser(userId,toUser);
        }

        [HttpPost]
        public void Post(Chat chat) => repository.AddChat(chat);
    }
}
