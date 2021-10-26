using signalRChatApiServer.Data;
using signalRChatApiServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace signalRChatApiServer.Repositories
{
    public class MainRepository : IRepository
    {
        private readonly TalkBackChatContext context;
        private List<User> users;
        private List<Message> messages;
        private List<Chat> chats;

        public MainRepository(TalkBackChatContext context)
        {
            this.context = context;
            users = context.Users.ToList();
        }
        public Task AddChat(Chat chat)
        {
            throw new NotImplementedException();
        }

        public Task AddMessage(Message message)
        {
            throw new NotImplementedException();
        }

        public Task AddUser(User user)
        {
            throw new NotImplementedException();
        }

        public Task DeleteChat(int id)
        {
            throw new NotImplementedException();
        }

        public Task DeleteMessage(int id)
        {
            throw new NotImplementedException();
        }

        public Task DeleteUser(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Chat>> GetChat(User userA, User userB)
        {
            throw new NotImplementedException();
        }

        public Task<Message> GetMessage(Message message)
        {
            throw new NotImplementedException();
        }

        public Task<List<Message>> GetMessagesFromUser(User user)
        {
            throw new NotImplementedException();
        }

        public Task<List<User>> GetUsers(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Chat> UpdateChat(Chat chat)
        {
            throw new NotImplementedException();
        }

        public Task<Message> UpdateMessage(Message message)
        {
            throw new NotImplementedException();
        }

        public Task<User> UpdateUser(User user)
        {
            throw new NotImplementedException();
        }
    }
}
