using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace signalRChatApiServer.Models
{
    public class Chat
    {
        public int ChatId { get; set; }
        public int UserAId { get; set; }
        public int UserBId { get; set; }
        public virtual ICollection<Message> Messages { get; set; }
    }
}