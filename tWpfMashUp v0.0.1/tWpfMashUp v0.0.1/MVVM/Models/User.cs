﻿using System.Collections.Generic;

namespace tWpfMashUp_v0._0._1.MVVM.Models
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public Status IsConnected { get; set; }
        public string HubConnectionString { get; set; }
    }
}