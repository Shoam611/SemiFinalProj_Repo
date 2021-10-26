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
        List<Message> GetMessages(Chat chat);
        Chat GetChat(User userA, User userB);

        #endregion

        #region Update
        void UpdateUser(User user);
        void UpdateChat(Chat chat);
        #endregion
    }
}
