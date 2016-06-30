using DataSource;
using PlatformManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PlatformManagement.Controllers.Bll
{
    public class BllWorkTask : BllBase
    {
        public static readonly string WORK_TASK = "WORK_TASK";
        public BllWorkTask() : base(WORK_TASK)
        {

        }

        public object AddWorkTaskInfo(WorkTask workTaskInfo)
        {
            var w = workTaskInfo.TaskInfo.FirstOrDefault();
            if (w == null)
            {
                return new
                {
                    result = false,
                    message = "无效的工作模块"
                };
            }
            var model = this.GetWorkTaskByGuid(workTaskInfo.Guid);

            if (!default(KeyValuePair<string, string>).Equals(model))
            {
                return new
                {
                    result = false,
                    message = "找不到该工作模块"
                };
            }

            bool isExit = false;

            model.Value.TaskInfo.ForEach(t =>
            {
                if (t.Guid == w.Guid)
                {
                    t.UseTime = w.UseTime;
                    t.Context = w.Context;
                    isExit = true;
                    return;
                }
            });

            if (!isExit)
            {
                model.Value.TaskInfo.Add(w);
            }

            return new
            {
                result = true,
                message = "success"
            };
        }
        public bool RemoveWorkTaskByKey(string uuid)
        {
            return RedisBase.HashRemove(this._key, uuid);
        }

        public KeyValuePair<string, WorkTask> GetWorkTaskByGuid(string guid)
        {
            var works = this.GetWorkTask();
            if (works == null)
            {
                return default(KeyValuePair<string, WorkTask>);
            }
            return works.Where(t => t.Key == guid).FirstOrDefault();
        }

        public bool AddWorkTask(WorkTask workTask)
        {
            if (string.IsNullOrEmpty(workTask.Guid))
            {
                workTask.SetGuid();
            }
            KeyValuePair<string, WorkTask> work = new KeyValuePair<string, WorkTask>(workTask.Guid, workTask);
            return RedisBase.HashSet(this._key, work.Key, work);
        }

        public List<KeyValuePair<string, WorkTask>> GetWorkTask()
        {
            return RedisBase.HashGetAll<KeyValuePair<string, WorkTask>>(this._key);
        }
    }
}