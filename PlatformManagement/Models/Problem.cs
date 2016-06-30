using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PlatformManagement.Models
{
    public class Problem
    {
        public string Id { get; set; }
        public string WorkTaskId { get; set; }
        public Account Duty { get; set; }
        public ProblemType ProblemType { get; set; }
        public ProblemResult? ProblemResult { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
          
    }
}