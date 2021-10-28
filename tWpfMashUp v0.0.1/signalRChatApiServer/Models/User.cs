using System.Collections.Generic;

namespace signalRChatApiServer.Models
{
    public class User //:IdentityUser
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public ICollection<Chat> Chats { get; set; }

    }
}
