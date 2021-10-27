using System.Collections.Generic;

namespace tWpfMashUp_v0._0._1.MVVM.Models
{
    public class Chat
    {
        public int ChatId { get; set; }
        public int UserAId { get; set; }
        public int UserBId { get; set; }
        public virtual ICollection<Message> Messages { get; set; }
    }
}