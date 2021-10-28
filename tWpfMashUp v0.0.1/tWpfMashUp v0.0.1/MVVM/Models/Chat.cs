using System.Collections.Generic;
using System.Linq;

namespace tWpfMashUp_v0._0._1.MVVM.Models
{
    public class Chat
    {
        public int Id { get; set; }

        public virtual ICollection<Message> Messages { get; set; }
        public virtual ICollection<UserModel> Users { get; set; }
    }
}