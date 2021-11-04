using System.Linq;
using signalRChatApiServer.Data;
using System.Collections.Generic;
using signalRChatApiServer.Models;
using signalRChatApiServer.Repositories.Infra;

namespace signalRChatApiServer.Repositories.Repos
{
    public class UsersReposatory : IUsersReposatory
    {
        private readonly TalkBackChatContext context;
        public UsersReposatory(TalkBackChatContext context)
        {
            this.context = context;
        }
        public User GetUser(int id) => context.Users.Find(id);// when sighning in /authenticating

        public List<User> GetAllUsers() => context.Users.ToList();//when fetching

        public User Authenticate(string username, string password) => (from user in context.Users
                                                                       where user.UserName == username && password == user.Password
                                                                       select user).FirstOrDefault();

        public int AddUser(User user)
        {
            var id = context.Users.Add(user).Entity.Id;
            context.SaveChanges();
            return id;
        }

        public void UpdateUser(User user)
        {
            var tempUser = context.Users.FirstOrDefault(u => u.Id == user.Id);
            if (tempUser == null) return;
            tempUser.IsConnected = user.IsConnected;
            tempUser.HubConnectionString = user.HubConnectionString;
            context.SaveChanges();
        }


    }
}
