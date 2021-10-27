using System;

namespace tWpfMashUp_v0._0._1.MVVM.Models
{
    public class Message
    {
        public int MessageId { get; set; }
        public string Content { get; set; }
        public DateTime Date { get; set; }

        public int UserId { get; set; }
        public virtual UserModel User { get; set; }

        public int ChatId { get; set; }
        public virtual Chat Chat { get; set; }
    }
}