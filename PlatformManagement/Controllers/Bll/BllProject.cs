using DataSource;
using PlatformManagement.Common;
using PlatformManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PlatformManagement.Controllers.Bll
{
    public class BllProject : BllBase
    {
        public static readonly string PROJECT = "PROJECT";

        public BllProject() : base(PROJECT)
        {

        }
       
        public bool AddProject(Project project)
        {
            project.SetGuid();
            KeyValuePair<string, Project> model = new KeyValuePair<string, Project>(project.Guid, project);
            if (model.Value.Task != null)
            {
                BllWorkTask bllWorkTask = new BllWorkTask();
                model.Value.Task.ForEach(t =>
                {
                    t.SetGuid();
                    t.PojectId = project.Guid;

                    bllWorkTask.AddWorkTask(t);
                    //if (t.TaskInfo != null)
                    //{
                    //    t.TaskInfo.ForEach(l =>
                    //    {
                    //        l.SetGuid();
                    //        l.WorkTaskId = t.Guid;
                    //    });

                    //}
                });
                model.Value.Task = null;
            }
            return RedisBase.HashSet<KeyValuePair<string, Project>>(this._key, model.Key, model);
        }

        public bool Update(Project project)
        {
            this.RemoveByKey(project.Guid);
            return this.AddProject(project);
        }
        public bool RemoveByKey(string key)
        {
            return RedisBase.HashRemove(this._key, key);
        }
       
        public List<KeyValuePair<string, Project>> GetProject()
        {
            var projects = RedisBase.HashGetAll<KeyValuePair<string, Project>>(this._key).ForIn(t =>
            {
                if (!string.IsNullOrEmpty(t.Value.StartTime))
                {
                    t.Value.RunTime = (DateTime.Now - Convert.ToDateTime(t.Value.StartTime)).Days;
                }
                t.Value.Progress = CalcProgress(t.Value.Task);
                t.Value.TotalTime = CalcTotalTime(t.Value.Task);
            });
            if (projects != null)
            {
                return projects.ToList();
            }
            return null;
        }

        private string CalcProgress(List<WorkTask> workTasks)
        {
            if (workTasks == null)
            {
                return "-1";
            }
            double item = 100 / workTasks.Count;
            return (workTasks.Count(t => t.IsComplete) * item).ToString();
        }

        private int CalcTotalTime(List<WorkTask> list)
        {
            if (list == null || list.Any())
            {
                return -1;
            }
            return list.ForIn(t =>
            {
                t.UseTime = t.TaskInfo.Sum(w => w.UseTime);
            }).Sum(t => t.UseTime);
        }

    }
}