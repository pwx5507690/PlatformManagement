using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DataSource;
using PlatformManagement.Models;

namespace PlatformManagement.Controllers
{
    public class TaskTargetController : BaseCommonController
    {
        public static string TASK_TARGET = "TASK_TARGET";
       [HttpPost]
        public object AddTaskTarget([FromBody] TaskTarget tageTarget)
        {
            return base.OperationToResult(() =>
            {
                return RedisBase.HashSet<TaskTarget>(TASK_TARGET, tageTarget.Guid, tageTarget);
            }); 
        }
        [HttpGet]
       public List<KeyValuePair<string, TaskTarget>> GetWorkTask()
        {
            return RedisBase.HashGetAll<KeyValuePair<string, TaskTarget>>(TASK_TARGET);
        }
    }
}
