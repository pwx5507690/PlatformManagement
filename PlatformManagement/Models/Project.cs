using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PlatformManagement.Models
{
    public class Project:BaseEnity
    {
        public string StartTime { get; set; }
        public string Name { get; set; }
        public string Progress { get; set; }
        public double Money { get; set; }
        public int RunTime { get; set; }
        public bool IsUse { get; set; }
        public List<Account> Member { get; set; }
        public int TotalTime { get; set; }
        public List<WorkTask> Task { get; set; }
    }
}