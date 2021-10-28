using System;
using signalRChatApiServer.Models;
using Microsoft.EntityFrameworkCore;

namespace signalRChatApiServer.Data
{
    public class TalkBackChatContext : DbContext
    {
        public TalkBackChatContext(DbContextOptions<TalkBackChatContext> options) : base(options) { }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Chat> Chats { get; set; }
        public virtual DbSet<Message> Messages { get; set; }
      
    }
}
