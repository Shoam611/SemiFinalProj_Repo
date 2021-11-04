using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tWpfMashUp_v0._0._1.MVVM.Models
{
    public class ChatUser
    {
        public int ChatId { get; set; }
        public Chat Chat { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
    }
}
