using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace signalRChatApiServer.Models
{
    public class Message
    {
        // own properties
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime Date { get; set; }
        // Navigation properties
        public string Name { get; set; }

        public int ChatId{ get; set; }
        public virtual Chat Chat { get; set; }
    }
}