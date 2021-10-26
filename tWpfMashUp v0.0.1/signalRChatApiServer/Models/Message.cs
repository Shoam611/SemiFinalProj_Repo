﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace signalRChatApiServer.Models
{
    public class Message
    {
        [Key]
        public int MessageId { get; set; }
        public string Content { get; set; }
        public DateTime Date { get; set; }
        [ForeignKey("UserId")]
        public virtual User UserId { get; set; }
        [ForeignKey("ChatId")]
        public virtual Chat ChatId { get; set; }
    }
}