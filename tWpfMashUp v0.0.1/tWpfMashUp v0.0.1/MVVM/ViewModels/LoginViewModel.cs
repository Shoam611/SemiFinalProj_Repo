using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using tWpfMashUp_v0._0._1.Core;
using tWpfMashUp_v0._0._1.MVVM.Models;

namespace tWpfMashUp_v0._0._1.MVVM.ViewModels
{
    public class LoginViewModel : ObservableObject
    {
        private string userName;
        public string UserName
        {
            get { return userName; }
            set { userName = value; onProppertyChange(); }
        }
        private string password;

        public string Password
        {
            get { return password; }
            set { password = value; onProppertyChange(); }
        }

        public RelayCommand LoginCommand { get; set; }
        public RelayCommand AuthenticateCommand { get; set; }
        public RelayCommand SighUpCommand { get; set; }
        public RelayCommand PasswordChangedCommand { get; set; }
        public LoginViewModel()
        {
            UserName = "Usename";
            SighUpCommand = new RelayCommand((o) => SighnUpHandler());
            PasswordChangedCommand = new RelayCommand((o) => HandlePasswordChanged(o as RoutedEventArgs));
            AuthenticateCommand = new RelayCommand(o => LogInHandler());
        }

        private async void SighnUpHandler()
        {
            var url = @"http://localhost:14795/Users";
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    var values = new Dictionary<string, string> { { "UserName", UserName }, { "Password", Password } };
                    var content = new StringContent(JsonConvert.SerializeObject(values), Encoding.UTF8, "application/json");
                    var response = await client.PostAsync(url, content);
                    var responseString = await response.Content.ReadAsStringAsync();//for debug purposes
                    response.EnsureSuccessStatusCode();
                    LogInHandler();
                }
                catch (Exception ex) { MessageBox.Show(ex.Message, "Failed to call server"); }
            }
        }
        private async void LogInHandler()
        {
            var url = @$"http://localhost:14795/Users?username={UserName}&password={Password}";
            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                var rawData = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<UserModel>(rawData);
                if (data != null)
                {
                    //add user  to store
                    LoginCommand?.Execute(null);
                }
                else
                {
                    MessageBox.Show("User not found");
                }
            }
        }

        void HandlePasswordChanged(RoutedEventArgs args) => Password = (args.Source as PasswordBox).Password;
    }
}

//var response = await client.GetAsync(url);
//response.EnsureSuccessStatusCode();
//if (response.IsSuccessStatusCode)
//{
//    var rawData = await response.Content.ReadAsStringAsync();
//    return rawData;
//}
//throw new Exception();