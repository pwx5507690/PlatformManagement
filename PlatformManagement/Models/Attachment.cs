using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PlatformManagement.Models
{
    public class Attachment:BaseEnity
    {
        public string FileName { get; set; }
        public string DateTime { get; set; }
    }
}