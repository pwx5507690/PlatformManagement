using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PlatformManagement.Models
{
    public class SessionCache : BaseEnity
    {
        public string Token { get; set; }
        public string Name { get; set; }
        public string Time { get; set; }
        public string IpAddress { get; set; }
    }
}