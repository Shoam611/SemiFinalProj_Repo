using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tWpfMashUp_v0._0._1.Sevices
{
    public class MessagesService
    {
        StoreService storeService;
        public MessagesService(StoreService storeService)
        {
            this.storeService = storeService;
        }
    }
}
