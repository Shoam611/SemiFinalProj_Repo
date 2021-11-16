using System;
using System.Collections.Generic;
using tWpfMashUp_v0._0._1.MVVM.Models.GameModels;

namespace tWpfMashUp_v0._0._1.MVVM.Models
{
    public class MessageRecivedEventArgs : EventArgs
    {
        public Message Massage { get; set; }
        public int ChatId { get; set; }
    }
    public class ContactLoggedEventArgs : EventArgs
    {
        public User User { get; set; }
        public bool IsLoggedIn { get; set; }
    }

    public class UserInvitedEventArgs : EventArgs
    {
        public User User { get; set; }
        public int ChatId { get; set; }
    }
    public class ChatRecivedEventArgs : EventArgs
    {
        public Chat NewChat { get; set; }
        public string ContactName { get; set; }
    }
    public class OpponentPlayedEventArgs : EventArgs
    {
        public MatrixLocation Source { get; set; }
        public MatrixLocation Destenation { get; set; }
    }

}
