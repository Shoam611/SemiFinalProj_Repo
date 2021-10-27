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
    public class AuthenticationService
    {

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
                    var data = JsonConvert.DeserializeObject<UserModel>(rawData);
                    if (data != null)
                    {
                        //add user  to store  
                        return true;
                    }
                }
                catch { MessageBox.Show("Failed To Call Server"); }
                return false;
            }
        }

    }
}
