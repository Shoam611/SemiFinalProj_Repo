﻿using Newtonsoft.Json;
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
    public class InvitesService
    {
        private readonly StoreService storeService;
        private readonly SignalRListenerService signalRListenerService;

        public InvitesService(StoreService storeService, SignalRListenerService signalRListenerService)
        {
            this.storeService = storeService;
            this.signalRListenerService = signalRListenerService;
            this.signalRListenerService.UserInvitedToGame += OnGameInvitation;
        }
        private async void OnGameInvitation(object sender, UserInvitedEventArgs eventArgs)
        {
            var mb = await Modal.ShowModal($"Start a game with {eventArgs.User.UserName} ?","Game Invitation", "Accept", "Deny");
            bool res;
            if (mb == "Accept") res = true; 
            else res = false;
                
            AcceptGameInviteAsync(eventArgs.ChatId, res); ;
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