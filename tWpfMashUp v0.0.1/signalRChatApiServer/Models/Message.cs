using System;

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
    }
}