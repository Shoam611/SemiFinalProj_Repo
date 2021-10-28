using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace signalRChatApiServer.Models
{
    public class Message
    {
        // own properties
        public int MessageId { get; set; }
        public string Content { get; set; }
        public DateTime Date { get; set; }

        // Navigation properties
        public string Name { get; set; }


        //[ForeignKey("UserId")]
        //public int UserId { get; set; }
        //public virtual User User { get; set; }

        //[ForeignKey("ChatId")]
        //public int ChatId { get; set; }
        //public virtual Chat Chat { get; set; }
    }
}