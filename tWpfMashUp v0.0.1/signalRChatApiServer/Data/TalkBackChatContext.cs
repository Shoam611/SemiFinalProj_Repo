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
       
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Chat>().HasData(
                new { Id = 1 },
                new { Id = 2 },
                new { Id = 3 }
                );
            modelBuilder.Entity<User>().HasData(
                new { Id = 1, IsConnected = Status.Offline, UserName = "User1", Password = "123", HubConnectionString = "dummy-c-string" },
                new { Id = 2, IsConnected = Status.Offline, UserName = "User2", Password = "123", HubConnectionString = "dummy-c-string" },
                new { Id = 3, IsConnected = Status.Offline, UserName = "User3", Password = "123", HubConnectionString = "dummy-c-string" }
                );
            modelBuilder.Entity<Message>().HasData(
                new { Id = 1, Content = "למה לא בעצם?", Date = DateTime.Now.AddMinutes(50), ChatId = 1, Name = "User1", },
                new { Id = 2, Content = "למה אתה כותב באנגלית?", Date = DateTime.Now.AddMinutes(37), ChatId = 1, Name = "User2", },
                new { Id = 3, Content = "Hi, how are you?", Date = DateTime.Now.AddMinutes(5), ChatId = 1, Name = "User1", },

                new { Id = 4, Content = "Q_Q everyone are against me", Date = DateTime.Now.AddMinutes(72), ChatId = 2, Name = "User1", },
                new { Id = 5, Content = "why are you so toxic?", Date = DateTime.Now.AddMinutes(49), ChatId = 2, Name = "User3", },
                new { Id = 6, Content = "User2 is such a bitch...", Date = DateTime.Now.AddMinutes(17), ChatId = 2, Name = "User1", },

                new { Id = 7, Content = "מה... איזה דביל", Date = DateTime.Now.AddMinutes(10), ChatId = 3, Name = "User2", },
                new { Id = 8, Content = "כי שאלתי למה הוא כותב באנגלית", Date = DateTime.Now.AddMinutes(3), ChatId = 3, Name = "User3", },
                new { Id = 9, Content = "למה יוזר1 אחד שונא אותך?", Date = DateTime.Now, ChatId = 3, Name = "User2", }
                );
            modelBuilder.Entity<Chat>()
                        .HasMany(c => c.Users)
                        .WithMany(u => u.Chats)
                        .UsingEntity<ChatUser>(
                            cu => cu.HasOne(cu => cu.User)
                                    .WithMany(u => u.ChatUsers)
                                    .HasForeignKey(cu => cu.UserId),
                            cu => cu.HasOne(cu => cu.Chat)
                                    .WithMany(c => c.ChatUsers)
                                    .HasForeignKey(cu => cu.ChatId),
                            cu =>
                            {
                                cu.HasKey(cu => new { cu.ChatId, cu.UserId });
                                cu.HasData(
                                    new { ChatId = 1, UserId = 1 },
                                    new { ChatId = 1, UserId = 2 },

                                    new { ChatId = 2, UserId = 1 },
                                    new { ChatId = 2, UserId = 3 },

                                    new { ChatId = 3, UserId = 2 },
                                    new { ChatId = 3, UserId = 3 }
                                );
                            }
                        );
          
        }
    }
}
