using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace signalRChatApiServer.Models
{
    public class Chat
    {
        [Key]
        public int ChatId { get; set; }

        public virtual ICollection<User> UsersInChat { get; set; }
        public virtual ICollection<Message> Messages { get; set; }
        //[ForeignKey("UserId")]
        //public int UserAId { get; set; }

        //[ForeignKey("UserId")]
        //public int UserBId { get; set; }

    }
}