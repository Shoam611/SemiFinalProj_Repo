﻿using System.Collections.Generic;
using System.Linq;
using tWpfMashUp_v0._0._1.Core;

namespace tWpfMashUp_v0._0._1.MVVM.Models
{
    public class Chat : ObservableObject
    {
        public int Id { get; set; }

        public virtual ICollection<Message> Messages { get; set; }
        public virtual ICollection<UserModel> Users { get; set; }
        public string UserANAme => Users.Last().UserName ?? "";
        public string UserBNAme => Users.First().UserName ?? "";
        string lastMessage = "";
        public string LastMessage
        {
            get
            {
                try { return Messages.Last().Content; }
                catch { return " "; }
            }
            set { lastMessage = value; onProppertyChange(); }

        }
    }
}