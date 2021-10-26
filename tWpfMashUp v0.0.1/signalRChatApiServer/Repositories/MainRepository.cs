using signalRChatApiServer.Data;
using signalRChatApiServer.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace signalRChatApiServer.Repositories
{
    public class MainRepository : IRepository
    {
        private readonly TalkBackChatContext context;
        private readonly List<User> users;
        private readonly List<Message> messages;
        private readonly List<Chat> chats;

        public MainRepository(TalkBackChatContext context)
        {
            this.context = context;
            users = context.Users.ToList();
            messages = context.Messages.ToList();
            chats = context.Chats.ToList();

            Debug.WriteLine("Repository loading!");
        }

        #region Add
        public void AddChat(Chat chat)
        {
            context.Chats.Add(chat);
            context.SaveChanges();
        }

        public void AddMessage(Message message)
        {
            context.Messages.Add(message);
            context.SaveChanges();
        }

        public void AddUser(User user)
        {
            context.Users.Add(user);
            context.SaveChanges();
        }
        #endregion

        #region Read
        public Chat GetChat(User userA, User userB) => context.Chats.Where(c => (c.UserA == userA && c.UserB == userB)).First();
        
        public Chat GetChatByID(int id) => context.Chats.Find(id);

        public List<Message> GetMessages(Chat chat) => context.Messages.Where(m => m.ChatId.Equals(chat.ChatId)).ToList();

        public User GetUser(int id) => context.Users.Find(id);

        #endregion

        #region Update
        public void UpdateChat(Chat chat)
        {
            var tempChat = context.Chats.Where(c => c.ChatId == chat.ChatId).First();
            tempChat.Messages = chat.Messages;
            tempChat.UserA = chat.UserA;
            tempChat.UserB = chat.UserB;
            context.SaveChanges();
        }

        public void UpdateUser(User user)
        {
            var tempUser = context.Users.Where(c => c.UserId == user.UserId).First();
            tempUser.ChatsA= user.ChatsA;
            tempUser.UserName= user.UserName;
            tempUser.Password= user.Password;
            context.SaveChanges();
        }
        #endregion
    }
}
