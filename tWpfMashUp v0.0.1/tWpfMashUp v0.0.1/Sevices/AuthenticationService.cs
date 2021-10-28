using Newtonsoft.Json;
using System;
using System.Linq;
using System.Text;
using System.Windows;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using tWpfMashUp_v0._0._1.MVVM.Models;
using System.Diagnostics;

namespace tWpfMashUp_v0._0._1.Sevices
{
    public class AuthenticationService
    {
        StoreService storeService;
        public AuthenticationService(StoreService storeService)
        {
            this.storeService = storeService;
        }

        public async Task<bool> CallServerToSighnUp(string username, string password)
        {
            var url = @"http://localhost:14795/Users";
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    var values = new Dictionary<string, string> { { "UserName", username }, { "Password", password } };
                    var content = new StringContent(JsonConvert.SerializeObject(values), Encoding.UTF8, "application/json");
                    var response = await client.PostAsync(url, content);
                    /*for debug purposes*/ var responseString = await response.Content.ReadAsStringAsync();
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
                    var loggedUser = JsonConvert.DeserializeObject<UserModel>(rawData);
                    
                    if (loggedUser != null)
                    {
                        storeService.Add(CommonKeys.LoggedUser.ToString(), loggedUser); return true;
                    }
                }
                catch { MessageBox.Show("Failed To Call Server"); }
                return false;
            }
        }

        public async Task<IEnumerable<Chat>> GetUsersChatsAsync(int userId)
        {
            var url = @$"http://localhost:14795/Chat?userId={userId}";
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    var response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();
                    var rawData = await response.Content.ReadAsStringAsync();
                    var chats = JsonConvert.DeserializeObject<IEnumerable<Chat>>(rawData);
                    //fetch user's chats List
                    if (chats != null)
                    {
                        Debug.WriteLine(chats);                        
                        return chats;
                    }
                }
                catch { MessageBox.Show("Failed To Call Server"); }
                return null;
            }
        }

    }
}
