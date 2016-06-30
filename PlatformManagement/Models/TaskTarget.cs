using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PlatformManagement.Models
{
    public class TaskTarget : BaseEnity
    {
        public string WorkTaskInfoId { get; set; }
        public string ProjectId { get; set; }
        public string WorkTaskId { get; set; }
        public string AccountId { get; set; }
        public string Name { get; set; }
        public string PredictEndTime { get; set; }
        public string CreateTime { get; set; }
        public bool IsOver { get; set; }
        public string SenseEndTime { get; set; }
        public string Context { get; set; }
    }
}