using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace signalRChatApiServer.Models
{
    public class Chat
    {
        [Key]
        public int ChatId { get; set; }
        public List<Message> Messages { get; set; }
        public User UserA { get; set; }
        public User UserB { get; set; }
    }
}