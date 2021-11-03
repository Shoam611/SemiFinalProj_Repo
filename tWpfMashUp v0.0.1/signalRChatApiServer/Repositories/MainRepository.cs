using System.Linq;
using signalRChatApiServer.Data;
using System.Collections.Generic;
using signalRChatApiServer.Models;

namespace signalRChatApiServer.Repositories
{
    public class MainRepository : IRepository
    {
        private readonly TalkBackChatContext context;


        public MainRepository(TalkBackChatContext context)
        {
            this.context = context;
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
        public int AddChat(Chat chat) //when openning a room
        {
            if (chat == null || chat.Users.Contains(null)) return 0;
            var e = context.Chats.Add(chat);
            context.SaveChanges();
            return e.Entity.Id;
        }

        public int AddMessage(Message message)//when sending a masssage
        {
            //var s = "☻";
            if (message.ChatId <= 0) return -1;
            var id = context.Messages.Add(message).Entity.Id;
            context.SaveChanges();
            return id;
        }

        public int AddUser(User user)//when sighning up
        {
            var id = context.Users.Add(user).Entity.Id;
            context.SaveChanges();
            return id;
        }
        #endregion

        #region Get
        public IEnumerable<Chat> GetUserChats(User user)//when sighning in
            => context.Chats.Where(c => c.Users.Contains(user));

        public IEnumerable<Chat> GetUserChatsById(int UserId) => context.Chats.Where(c => c.Users.Contains(GetUser(UserId)));

        public IEnumerable<Message> GetMessages(int chatId) //when loading a chat
            => context.Messages.Where(m => m.ChatId == chatId);

        public User GetUser(int id) => context.Users.Find(id);// when sighning in / authenticating

        public User Authenticate(string username, string password)
         => context.Users.Where(u => u.UserName == username && password == u.Password).FirstOrDefault();

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
            var tempUser = context.Users.FirstOrDefault(u => u.Id == user.Id);
            if (tempUser == null) return;
            tempUser.IsConnected = user.IsConnected;
            tempUser.HubConnectionString = user.HubConnectionString;
            context.SaveChanges();
        }

        public Chat GetChat(int id)
        {
            return context.Chats.Find(id);
        }

        public List<User> GetAllUsers() => context.Users.ToList();

        public Chat GetChatByMessage(int messageId)
        {
            return context.Chats.Where(c => c.Messages.Any(m => m.Id == messageId)).FirstOrDefault();
        }

        public Chat CreateNewChat(int user1Id, int user2Id)
        {
            var chat = new Chat { Users = new List<User> { GetUser(user1Id), GetUser(user2Id) }, Messages = new List<Message>() };
            var id = AddChat(chat);
            chat.Id = id;
            return chat;
        }

        public bool IsChatExist(int user1Id, int user2Id, out Chat c)
        {
            var qchat = (from chat in context.Chats
                     where chat.Users.Contains(GetUser(user2Id)) &&
                            chat.Users.Contains(GetUser(user1Id))
                     select chat).Take(1);
            if(qchat.Any())
            {
                c = qchat.First(); return true;
            }
            else
            {
                c = CreateNewChat(user1Id,user2Id);   return false;
            }
        }

        #endregion
    }
}
//☻