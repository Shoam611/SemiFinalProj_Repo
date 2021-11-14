using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tWpfMashUp_v0._0._1.Sevices
{
    public class GameService
    {
        private readonly StoreService storeService;
        private readonly SignalRListenerService signalRListenerService;

        public GameService(StoreService storeService, SignalRListenerService signalRListenerService)
        {
            this.storeService = storeService;
            this.signalRListenerService = signalRListenerService;
        }
    }
}
