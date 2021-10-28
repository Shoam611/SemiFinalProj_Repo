using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using tWpfMashUp_v0._0._1.MVVM.Models;

namespace tWpfMashUp_v0._0._1.Sevices
{
    public class ChatsService
    {
        StoreService store;
        public ChatsService(StoreService store) => this.store = store;


        public async Task<Chat> GetRandomChatAsync()
        {
            var id = ((UserModel)store.Get(CommonKeys.LoggedUser.ToString())).Id;
            var url = @$"http://localhost:14795/Chat?userId={2} ";
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    var response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();
                    var resString = await response.Content.ReadAsStringAsync();
                    var chat = JsonConvert.DeserializeObject<Chat>(resString);
                    return chat;
                }
                catch { MessageBox.Show("Failed To Call Server"); }
                return null;
            }
        }
    }
}
