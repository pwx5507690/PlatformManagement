using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PlatformManagement.Models
{
    public class WorkTask : BaseEnity
    {
        public string PojectId { get; set; }
        public string Name { get; set; }
        public List<Account> Principal { get; set; }
        public string StartTime { get; set; }
        public List<WorkTaskInfo> TaskInfo { get; set; }
        public int UseTime { get; set; }
        public bool IsComplete { get; set; }
    }
}