﻿using System.Collections.Generic;

namespace signalRChatApiServer.Models
{
    public class Chat
    {       
        public int Id { get; set; }
        public string GameAproval { get; set; }
        public ICollection<User> Users { get; set; }
        public ICollection<Message> Messages { get; set; }

       public List<ChatUser> ChatUsers { get; set; }
    }
}