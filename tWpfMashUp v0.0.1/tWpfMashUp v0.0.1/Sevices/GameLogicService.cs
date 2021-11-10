using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using tWpfMashUp_v0._0._1.Assets.Components.CustomModal;
using tWpfMashUp_v0._0._1.MVVM.Models;

namespace tWpfMashUp_v0._0._1.Sevices
{
    public class GameLogicService
    {
        private readonly StoreService storeService;
        private readonly SignalRListenerService signalRListenerService;

        public GameLogicService(StoreService storeService, SignalRListenerService signalRListenerService)
        {
            this.storeService = storeService;
            this.signalRListenerService = signalRListenerService;
            this.signalRListenerService.UserInvitedToGame += OnGameInvitation;
        }
        private void OnGameInvitation(object sender, UserInvitedEventArgs eventArgs)
        {
            var mb = Modal.ShowModal($"Start a game with {eventArgs.User.UserName} ?", "Game Invitation", "Accept", "Deny");
            if (mb == "Accept")
            {
                AcceptGameInviteAsync(eventArgs.ChatId, true);
            }
            else
            {
                DenyGameInviteAsync(eventArgs.ChatId, true);
            }
        }

        private void DenyGameInviteAsync(int chatId, bool isAccepted)
        {
            //throw new NotImplementedException();
        }

        private async void AcceptGameInviteAsync(int chatId, bool isAccepted)
        {
            var me = storeService.Get(CommonKeys.LoggedUser.ToString()) as User;
            var url = $@"http://localhost:14795/Invites?chatId={chatId}&accepted={isAccepted}";
            using (var client = new HttpClient())
            {
                try
                {
                    var response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();
                }
                catch { }
            }

            //server will push web socket to the two users
            //from the background the two users will pop to the game
            //impliment through chat?
        }

        public async Task CallServerForOtherUserInvite()
        {
            var url = @"http://localhost:14795/Invites";
            using HttpClient client = new();
            try
            {
                Chat currentChat = storeService.Get(CommonKeys.CurrentChat.ToString()) as Chat;
                if (currentChat == null) { Modal.ShowModal("No User Selected To Play With!"); }

                var content = new StringContent(JsonConvert.SerializeObject(currentChat), Encoding.UTF8, "application/json");
                var response = await client.PutAsync(url, content);
                response.EnsureSuccessStatusCode();
            }
            catch { Modal.ShowModal("Failed to call server"); }
        }

    }
}
