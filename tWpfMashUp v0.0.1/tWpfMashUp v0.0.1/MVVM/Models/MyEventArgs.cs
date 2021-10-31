using System;

namespace tWpfMashUp_v0._0._1.MVVM.Models
{
  public      class MessageRecivedEventArgs : EventArgs
    {
        public Massage Massage{ get; set; }
        public int ChatID{ get; set; }
    }
}
