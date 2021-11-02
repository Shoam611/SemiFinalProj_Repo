using signalRChatApiServer.Models;
using System.Collections.Generic;

namespace signalRChatApiServer.Repositories
{
    public interface IRepository
    {
        void AddChat(Chat chat);
        void AddMessage(Message message);
        void AddUser(User user);
        User Authenticate(string username, string password);
        IEnumerable<Message> GetMessages(int chatId);
        User GetUser(int id);
        Chat GetChatByMessage(int messageId);
        IEnumerable<Chat> GetUserChats(User user);
        IEnumerable<Chat> GetUserChatsById(int UserId);
        Chat CreateChatWithUser(int userId, int toUser);
        void UpdateChat(Chat chat);
        void UpdateUser(User user);
        Chat GetChat(int id);
        List<User> GetAllUsers();
    }
}