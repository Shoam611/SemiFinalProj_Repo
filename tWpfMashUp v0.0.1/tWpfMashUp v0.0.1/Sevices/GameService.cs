using Castle.Core;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using tWpfMashUp_v0._0._1.Assets.Components.CustomModal;
using tWpfMashUp_v0._0._1.MVVM.Models;
using tWpfMashUp_v0._0._1.MVVM.Models.GameModels;

namespace tWpfMashUp_v0._0._1.Sevices
{
    public class GameService
    {
        private readonly StoreService store;
        private readonly SignalRListenerService signalRListenerService;

        public GameService(StoreService storeService, SignalRListenerService signalRListenerService)
        {
            this.store = storeService;
            this.signalRListenerService = signalRListenerService;
        }
        public async Task UpdateServerMove(Pair<MatrixLocation, MatrixLocation> actionUpdate)
        {
            var chatId = (store.Get(CommonKeys.CurrentChat.ToString()) as Chat).Id;
            var userId = (store.Get(CommonKeys.LoggedUser.ToString()) as User).Id;
            var actionUpdateobj = new ActionUpdateModel
            {
                SourceRow = actionUpdate.First.Row,
                SourceCol = actionUpdate.First.Col,
                DestenationRow = actionUpdate.Second.Row,
                DestenationCol = actionUpdate.Second.Col,
                ChatId = chatId,
                UserId = userId
            };
            string actionUpdateString = JsonConvert.SerializeObject(actionUpdateobj);
            Debug.WriteLine(actionUpdateString);
            var url = $@"http://localhost:14795/Game";
            try
            {
                using (HttpClient client = new())
                {
                    var content = new StringContent(actionUpdateString, Encoding.UTF8, "application/json");
                    var res = await client.PostAsync(url, content);
                    res.EnsureSuccessStatusCode();
                }
            }
            catch { Modal.ShowModal("Unknon error has accured"); }
        }
    }
}
