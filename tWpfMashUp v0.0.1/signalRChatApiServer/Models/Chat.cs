using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace signalRChatApiServer.Models
{
    public class Chat
    {

        public int ChatId { get; set; }

        [ForeignKey("UserA")]
        public int UserAId { get; set; }

        [ForeignKey("UserB")]
        public int UserBId { get; set; }
        public virtual List<Message> Messages { get; set; }
    }
}