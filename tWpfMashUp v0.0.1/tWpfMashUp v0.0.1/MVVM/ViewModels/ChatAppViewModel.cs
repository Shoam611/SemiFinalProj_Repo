using System.Collections.Generic;
using tWpfMashUp_v0._0._1.Core;

namespace tWpfMashUp_v0._0._1.MVVM.ViewModels
{
    public class ChatAppViewModel
    {
        public RelayCommand GoToGameCommand { get; set; }
        public List<string> OnlineContacts  { get; set; }
        public List<string> OfflineContacts { get; set; }        
        public string SelectedContact { get; set; }
        public ChatAppViewModel()
        {
            OnlineContacts = new List<string> {"Samual","Yehuda","Rafael" };
            OfflineContacts = new List<string> { "Samual", "Yehuda", "Rafael" }; ;
            //GoToGameCommand 
        }   
    }
}
