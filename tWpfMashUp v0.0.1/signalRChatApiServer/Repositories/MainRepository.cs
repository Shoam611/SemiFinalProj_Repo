using signalRChatApiServer.Data;
using signalRChatApiServer.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace signalRChatApiServer.Repositories
{
    public class MainRepository : IRepository
    //: IRepository
    {
        private TalkBackChatContext context;
        private IEnumerable<Chat> chats;
        private IEnumerable<Message> messages;
        private IEnumerable<User> users;

        public MainRepository(TalkBackChatContext context)
        {
            this.context = context;
        }
        public void PullData()
        {
            chats = context.Chats.ToList();
            users = context.Users.ToList();
            messages = context.Messages.ToList();
        }
        #region Create
        public Chat CreateChatWithUser(int userId, int toUser)
        {
            if (context.Users.ToArray().Length < 2 ||
                userId == toUser ||
                context.Users.Find(userId) == null ||
                context.Users.Find(toUser) == null)
                return null;
            var isExist = from chat in context.Chats
                          where chat.Users.Where(u => u.Id == userId).Any()
                             && chat.Users.Where(u => u.Id == userId).Any()
                          select chat;
            if (isExist.Any()) return isExist.FirstOrDefault();
            var userA = GetUser(userId);
            var userB = GetUser(toUser);
            var newChat = new Chat { Users = new List<User> { userA, userB } };
            AddChat(newChat);
            return newChat;
        }
        public void AddChat(Chat chat) //when openning a room
        {
            if (chat == null || chat.Users.Contains(null)) return;
            context.Chats.Add(chat);
            context.SaveChanges();
        }

        public void AddMessage(Message message)//when sending a masssage
        {
            //var s = "☻";
            context.Messages.Add(message);
            context.SaveChanges();
        }

        public void AddUser(User user)//when sighning up
        {
            context.Users.Add(user);
            context.SaveChanges();
        }
        #endregion

        #region Get
        public IEnumerable<Chat> GetUserChats(User user)//when sighning in
            => context.Chats.Where(c => c.Users.Contains(user));

        public IEnumerable<Chat> GetUserChatsById(int UserId) => context.Chats.Where(c => c.Users.Contains(GetUser(UserId)));

        public IEnumerable<Message> GetMessages(int chatId) //when loading a chat
            => context.Chats.Find(chatId).Messages;

        public User GetUser(int id) => context.Users.Find(id);// when sighning in / authenticating

        public User Authenticate(string username, string password)
            => context.Users.Where(u => u.UserName == username && password == u.Password).FirstOrDefault();

        public Chat GetChatByMessage(int messageId) => context.Chats.Where(c => c.Messages.Any(m => m.Id == messageId)).FirstOrDefault();

        #endregion

        #region Update
        public void UpdateChat(Chat chat)
        {
            var tempChat = context.Chats.Where(c => c.Id == chat.Id).FirstOrDefault();
            if (tempChat == null) return;
            tempChat.Messages = chat.Messages;
            tempChat.Id = chat.Id;
            tempChat.Users = chat.Users;
            context.SaveChanges();
        }

        public void UpdateUser(User user)
        {
            var tempUser = context.Users.Where(c => c.Id == user.Id).First();
            if (tempUser == null) return;
            tempUser.Id = user.Id;
            tempUser.UserName = user.UserName;
            tempUser.Password = user.Password;
            context.SaveChanges();
        }

        #endregion
    }
}
//☻