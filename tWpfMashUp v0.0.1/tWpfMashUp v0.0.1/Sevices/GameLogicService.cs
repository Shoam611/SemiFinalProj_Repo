using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using tWpfMashUp_v0._0._1.MVVM.Models;

namespace tWpfMashUp_v0._0._1.Sevices
{
    public class GameLogicService
    {
        private readonly StoreService storeService;
        public GameLogicService(StoreService storeService)
        {
            this.storeService = storeService;
        }
        public async Task<bool> CallServerForOtherUserInvite()
        {
            var url = @"http://localhost:14795/Game";
            using HttpClient client = new();
            try
            {
                if (storeService.Get(CommonKeys.WithUser.ToString()) is not User invitedUser) { MessageBox.Show("No User Selected To Play With!"); return false; }

                var content = new StringContent(JsonConvert.SerializeObject(invitedUser), Encoding.UTF8, "application/json");
                var response = await client.PutAsync(url, content);
                response.EnsureSuccessStatusCode();
                return true;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Failed to call server"); return false; }
        }


        //void AddItem(object item)
        //{
        //    Thread.Sleep(3000);
        //}
        //public async void AddItemAsync(object item)
        //{
        //    Task.Run(() => { AddItem(item); });
        //}
    }
}
