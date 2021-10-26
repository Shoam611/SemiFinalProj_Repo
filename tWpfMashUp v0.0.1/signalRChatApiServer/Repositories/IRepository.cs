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
        Task AddUser(User user);
        Task AddMessage(Message message);
        Task AddChat(Chat chat);
        #endregion

        #region Read
        Task<List<User>> GetUsers(int id);
        Task<Message> GetMessage(Message message);
        Task<List<Message>> GetMessagesFromUser(User user);
        Task<List<Chat>> GetChat(User userA, User userB);
        #endregion

        #region Update
        Task<User> UpdateUser(User user);
        Task<Message> UpdateMessage(Message message);
        Task<Chat> UpdateChat(Chat chat);
        #endregion

        #region Delete
        Task DeleteUser(int id);
        Task DeleteMessage(int id);
        Task DeleteChat(int id);
        #endregion
    }
}
