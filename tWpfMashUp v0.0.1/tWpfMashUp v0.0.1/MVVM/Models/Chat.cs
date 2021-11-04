using System.Collections.Generic;
using System.Linq;
using tWpfMashUp_v0._0._1.Core;

namespace tWpfMashUp_v0._0._1.MVVM.Models
{
    public class Chat : ObservableObject
    {
        public int Id { get; set; }

        public ICollection<User> Users { get; set; }
        public ICollection<Massage> Messages { get; set; }

        public List<ChatUser> ChatUsers { get; set; }

        //private string lastMessage = "";
        //public string LastMessage
        //{
        //    get
        //    {
        //        try { return Messages.Last().Content; }
        //        catch { return " "; }
        //    }
        //    set { lastMessage = value; onProppertyChange(); }
        //}
    }
}