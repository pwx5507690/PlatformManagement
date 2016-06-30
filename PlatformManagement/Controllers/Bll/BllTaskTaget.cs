using DataSource;
using System;
using System.Collections.Generic;
using PlatformManagement.Models;
using System.Linq;
using System.Web;

namespace PlatformManagement.Controllers.Bll
{
    public class BllTaskTaget : BllBase
    {
        public static readonly string TASK_TARGET = "TASK_TARGET";

        public BllTaskTaget() : base(TASK_TARGET)
        {

        }

        public List<KeyValuePair<string, TaskTarget>> GetTaskTarget()
        {
            return RedisBase.HashGetAll<KeyValuePair<string, TaskTarget>>(this._key);
        }

        public bool AddTaskTarget(TaskTarget taskTarget)
        {
            if (string.IsNullOrEmpty(taskTarget.Guid))
            {
                taskTarget.SetGuid();
            }
            taskTarget.CreateTime = DateTime.UtcNow.ToString();
            KeyValuePair<string, TaskTarget> model = new KeyValuePair<string, TaskTarget>(taskTarget.Guid, taskTarget);
            return RedisBase.HashSet<KeyValuePair<string, TaskTarget>>(this._key, taskTarget.Guid, model);
        }
    }
}