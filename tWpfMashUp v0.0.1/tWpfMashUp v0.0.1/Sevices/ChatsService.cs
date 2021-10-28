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
        public ChatsService()
        {

        }


        public async Task<Chat> GetRandomChatAsync()
        {
            var url = @$"http://localhost:14795/Users";
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    var response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();
                    var rawData = await response.Content.ReadAsStringAsync();
                    var loggedUser = JsonConvert.DeserializeObject<UserModel>(rawData);

                    if (loggedUser != null)
                    {
                    }
                }
                catch { MessageBox.Show("Failed To Call Server"); }
                return null;
            }
        }
    }
}
