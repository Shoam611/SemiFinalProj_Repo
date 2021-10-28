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
    public class MessagesService
    {
        StoreService storeService;
        public MessagesService(StoreService storeService)
        {
            this.storeService = storeService;
        }

        public async Task<bool> CallServerToAddMessage(string message)
        {
            var url = @"http://localhost:14795/Chat";
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    var msg = new Message { Content = message, Date = DateTime.Now, Name = ((UserModel)storeService.Get(CommonKeys.LoggedUser.ToString())).UserName };
                    var chat = ((Chat)storeService.Get(CommonKeys.CurrentChat.ToString()));
                    if (chat.Messages == null)
                        chat.Messages = new List<Message>();
                    chat.Messages.Add(msg);
                    var content = new StringContent(JsonConvert.SerializeObject(chat), Encoding.UTF8, "application/json");
                    var response = await client.PutAsync(url, content);
                    //Update ChatThread

                    //For Debug
                    var responseString = await response.Content.ReadAsStringAsync();
                    response.EnsureSuccessStatusCode();
                    return true;
                }
                catch (Exception ex) { MessageBox.Show(ex.Message, "Failed to call server"); }
                return false;
            }
        }
    }
}
