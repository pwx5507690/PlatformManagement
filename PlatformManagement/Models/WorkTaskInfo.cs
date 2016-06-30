using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PlatformManagement.Models
{
    public class WorkTaskInfo : BaseEnity
    {
        public string AccountId { get; set; }
        public string WorkTaskId { get; set; }
        public int UseTime { get; set; }
        public string Context { get; set; }
    }
}