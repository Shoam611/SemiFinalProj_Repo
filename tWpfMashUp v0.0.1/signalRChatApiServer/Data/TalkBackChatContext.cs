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
        public TalkBackChatContext(DbContextOptions<TalkBackChatContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<Message> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                new { UserId = 1, UserName = "user1", Password = "1234" },
                new { UserId = 2, UserName = "user2", Password = "1234" },
                new { UserId = 3, UserName = "user3", Password = "1234" },
                new { UserId = 4, UserName = "user4", Password = "1234" }
                );

            modelBuilder.Entity<Chat>().HasData(
              new { ChatId = 1, UserAId = 1, UserBId = 2 },
              new { ChatId = 2, UserAId = 1, UserBId = 3 },
              new { ChatId = 3, UserAId = 2, UserBId = 3 },
              new { ChatId = 4, UserAId = 2, UserBId = 4 },
              new { ChatId = 5, UserAId = 3, UserBId = 4 }
              );

            modelBuilder.Entity<Message>().HasData(
              new { MessageId = 1, Content = "dummy-content", ChatId = 1, Date = DateTime.Now, UserId = 1 },
              new { MessageId = 2, Content = "dummy-content", ChatId = 1, Date = DateTime.Now.AddDays(1), UserId = 2 },
              new { MessageId = 3, Content = "dummy-content", ChatId = 2, Date = DateTime.Now, UserId = 1 },
              new { MessageId = 4, Content = "dummy-content", ChatId = 2, Date = DateTime.Now.AddDays(1), UserId = 3 },
              new { MessageId = 5, Content = "dummy-content", ChatId = 3, Date = DateTime.Now, UserId = 2 },
              new { MessageId = 6, Content = "dummy-content", ChatId = 3, Date = DateTime.Now.AddDays(1), UserId = 3 },
              new { MessageId = 7, Content = "dummy-content", ChatId = 4, Date = DateTime.Now, UserId = 2 },
              new { MessageId = 8, Content = "dummy-content", ChatId = 4, Date = DateTime.Now.AddDays(1), UserId = 4 },
              new { MessageId = 9, Content = "dummy-content", ChatId = 5, Date = DateTime.Now, UserId = 3 },
             new { MessageId = 10, Content = "dummy-content", ChatId = 5, Date = DateTime.Now.AddDays(1), UserId = 4 }
              );

        }
    }

}
