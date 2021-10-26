using Microsoft.EntityFrameworkCore;
using signalRChatApiServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace signalRChatApiServer.Data
{
    public class TalkBackChatContext : DbContext
    {
        public TalkBackChatContext(DbContextOptions<TalkBackChatContext> options) : base(options){}

        public DbSet<User> Users { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<Message> Messages { get; set; }
    }
}
