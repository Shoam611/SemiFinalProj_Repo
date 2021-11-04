using signalRChatApiServer.Models;
using System.Collections.Generic;

namespace signalRChatApiServer.Repositories.Infra
{
    public interface IRepository
    {
        int AddChat(Chat chat);
        int AddMessage(Message message);
        int AddUser(User user);
        User Authenticate(string username, string password);
        IEnumerable<Message> GetMessages(int chatId);
        User GetUser(int id);
        IEnumerable<Chat> GetUserChats(User user);
        IEnumerable<Chat> GetUserChatsById(int UserId);
        Chat CreateChatWithUser(int userId, int toUser);
        void UpdateChat(Chat chat);
        void UpdateUser(User user);
        Chat GetChat(int id);
        List<User> GetAllUsers();
        bool IsChatExist(int user1Id, int user2Id, out Chat c);
        Chat CreateNewChat(int user1Id, int user2Id);
    }
}