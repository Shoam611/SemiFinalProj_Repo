using signalRChatApiServer.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace signalRChatApiServer.Repositories
{
    /// <summary>
    /// CRUD:
    /// Create, Read, Update, Delete
    /// </summary>
    public interface IRepository
    {
        #region Create
        void AddUser(User user);
        void AddMessage(Message message);
        void AddChat(Chat chat);
        #endregion

        #region Read
        User GetUser(int id);
        List<Message> GetMessages(int chatId);
        IEnumerable<Chat> GetUserChatsById(int id);
        List<Message> GetAllMessages();
        Chat GetChat(int userA, int userB);
        User Authenticate(string username, string password);

        #endregion

        #region Update
        void UpdateUser(User user);
        void UpdateChat(Chat chat);
        #endregion
    }
}
