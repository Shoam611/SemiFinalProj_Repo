using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace signalRChatApiServer.Models
{
    public class Chat
    {
        [Key]
        public int ChatId { get; set; }

        [ForeignKey("UserA")]
        public virtual User UserA { get; set; }

        [ForeignKey("UserB")]
        public virtual User UserB { get; set; }
        public virtual List<Message> Messages { get; set; }
    }
}