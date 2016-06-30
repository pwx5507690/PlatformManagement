using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace PlatformManagement.Models
{
    public class Account
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Img { get; set; }
        public string Mail { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string AccountName { get; set; }
        public bool IsUse { get; set; }
        public DateTime Age { get; set; }
        public string Address { get; set; }
        public AccountType AccountType { get; set; }
       
    }
}