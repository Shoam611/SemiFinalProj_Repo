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
        private readonly IEnumerable<Chat> chats;
        private readonly IEnumerable<Message> messages;
        private readonly IEnumerable<User> users;

        public MainRepository(TalkBackChatContext context)
        {
            this.context = context;
            chats = context.Chats.ToList();
            messages = context.Messages.ToList();
            users = context.Users.ToList();

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
        public Chat GetChat(int userA, int userB) => context.Chats.Where(c => c.UserAId == userA && c.UserBId == userB).FirstOrDefault();

        public Chat GetChatByID(int id) => context.Chats.Find(id);

        public List<Message> GetMessages(int chatId) => context.Messages.Where(m => m.ChatId == chatId).ToList();

        public List<Message> GetAllMessages() => context.Messages.ToList();

        public User GetUser(int id)
        {
            var newUser = context.Users.Find(id);
            var chatsIntoUser = chats.Where(c => c.UserAId == id || c.UserAId == id).ToList();
            newUser.Chats = chatsIntoUser;
            return newUser;
        }

        #endregion

        #region Update
        public void UpdateChat(Chat chat)
        {
            var tempChat = context.Chats.Where(c => c.ChatId == chat.ChatId).FirstOrDefault();
            if (tempChat == null) return;
            tempChat.Messages = chat.Messages;
            tempChat.UserAId = chat.UserAId;
            tempChat.UserBId = chat.UserBId;
            context.SaveChanges();
        }

        public void UpdateUser(User user)
        {
            var tempUser = context.Users.Where(c => c.UserId == user.UserId).First();
            if (tempUser == null) return;
            tempUser.Chats = user.Chats;
            tempUser.UserName = user.UserName;
            tempUser.Password = user.Password;
            context.SaveChanges();
        }
        #endregion

        public User Authenticate(string username, string password) => context.Users.Where(u => u.UserName == username && password == u.Password).FirstOrDefault();

        public IEnumerable<Chat> GetUserChatsById(int UserId) => chats.Where(c => c.UserAId == UserId || c.UserBId == UserId).ToList();
    }
}
