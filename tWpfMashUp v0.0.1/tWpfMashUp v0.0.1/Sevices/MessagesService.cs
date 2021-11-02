﻿using System;
using System.Linq;
using System.Text;
using System.Windows;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using tWpfMashUp_v0._0._1.MVVM.Models;

namespace tWpfMashUp_v0._0._1.Sevices
{
    public class MessagesService
    {
        private readonly StoreService storeService;
        public MessagesService(StoreService storeService)
        {
            this.storeService = storeService;
        }

        public async Task<bool> CallServerToAddMessage(string message)
        {
            var url = @"http://localhost:14795/Messages";
            using HttpClient client = new();
            try
            {
                var msg = new Massage { Content = message, Date = DateTime.Now, Name = ((User)storeService.Get(CommonKeys.LoggedUser.ToString())).UserName };
                var chat = (Chat)storeService.Get(CommonKeys.CurrentChat.ToString());
                if(chat == null)
                {
                    MessageBox.Show("No Chat Selected for messages"); return false;
                }
                if (chat.Messages == null)
                    chat.Messages = new List<Massage>();
                
                var json = JsonConvert.SerializeObject(msg);
                var content = new StringContent(JsonConvert.SerializeObject(msg), Encoding.UTF8, "application/json");
                var userConnection = storeService.Get(CommonKeys.WithUser.ToString()).HubConnectionString;
                var response = await client.PostAsync(url, content);

                //Update ChatThread
                return true;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Failed to call server"); return false; }
        }

        public async Task<List<Massage>> GetChatMassages(int chatId)
        {
            var url = @$"http://localhost:14795/Messages?chatId={chatId}";
            using HttpClient client = new();
            try
            {
                var response = await client.GetAsync(url);
                var readData = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<List<Massage>>(readData);
                return data;
            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.Message, "Failed to call server"); 
                return null; 
            }
        }
    }
}
