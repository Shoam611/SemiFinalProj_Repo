using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace signalRChatApiServer.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public List<User> Friends { get; set; }
        public List<Chat> Chats { get; set; }
    }
}
