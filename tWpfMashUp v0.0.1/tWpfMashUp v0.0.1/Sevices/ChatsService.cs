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
        readonly StoreService store;
        public ChatsService(StoreService store) => this.store = store;


        public async Task<Chat> GetChatAsync(int userToId)
        {
            var contacts = store.Get(CommonKeys.Contacts.ToString()) as List<UserModel>;
            if (contacts != null && contacts.Where(u => u.Id == userToId).Any()) return null;

            var id = ((UserModel)store.Get(CommonKeys.LoggedUser.ToString())).Id;
            var url = @$"http://localhost:14795/Chat?userId={id}&toUserId={userToId} ";
            Chat chat;            
            using (HttpClient client = new())
            {
                try
                {
                    var response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();
                    var resString = await response.Content.ReadAsStringAsync();
                    chat = JsonConvert.DeserializeObject<Chat>(resString);
                }
                catch { MessageBox.Show("Failed To Get Chat"); return null; }
            }
            if (chat == null)
            {
                MessageBox.Show("Cannot create Chat, Chat already exist ");
                return null;
            }
            if (chat.Messages == null) { chat.Messages = new List<Message>(); }

            var contact = chat.Users.Where(u => u.Id != id).First();
            chat.Contact = contact.UserName;
            if (contacts == null) contacts = new List<UserModel>();
            contacts.Add(contact);
            store.Add(CommonKeys.Contacts.ToString(), contacts);
            var chats = store.Get(CommonKeys.Chats.ToString()) as List<Chat>;
            if (chats == null) chats = new List<Chat>();
            chats.Add(chat);
            store.Add(CommonKeys.Chats.ToString(), chat);
            return chat;
        }

    }

}