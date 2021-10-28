using tWpfMashUp_v0._0._1.MVVM.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using System.Windows;
using System.Text;
using System;

namespace tWpfMashUp_v0._0._1.Sevices
{
    public class AuthenticationService
    {
        StoreService storeService;
        public AuthenticationService(StoreService storeService)
        {
            this.storeService = storeService;
        }

        public async Task<bool> CallServerToSignUp(string username, string password)
        {
            var url = @"http://localhost:14795/Users";
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    var values = new Dictionary<string, string> { { "UserName", username }, { "Password", password } };
                    var content = new StringContent(JsonConvert.SerializeObject(values), Encoding.UTF8, "application/json");
                    var response = await client.PostAsync(url, content);
                    /*for debug purposes*/
                    var responseString = await response.Content.ReadAsStringAsync();
                    response.EnsureSuccessStatusCode();
                    return true;
                }
                catch (Exception ex) { MessageBox.Show(ex.Message, "Failed to call server"); }
                return false;
            }
        }

        public async Task<bool> LoginAsync(string username, string password)
        {
            var url = @$"http://localhost:14795/Users?username={username}&password={password}";
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    var response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();
                    var rawData = await response.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<UserModel>(rawData);
                    if (data != null)
                    {
                        storeService.Add(CommonKeys.LoggedUser.ToString(), data); return true;
                    }
                }
                catch { MessageBox.Show("Failed To Call Server"); }
                return false;
            }
        }
    }
}
