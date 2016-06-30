using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DataSource;
using PlatformManagement.Common;
using PlatformManagement.Controllers.Bll;
using PlatformManagement.Models;

namespace PlatformManagement.Controllers
{
    public class WorkTaskController : BaseCommonController
    {
        public readonly BllWorkTask _bllWorkTask = new BllWorkTask();

        public readonly BllProject _bllProject = new BllProject();

        public WorkTaskController()
        {
            
        }

        [HttpPost]
        public object AddWorkTaskInfo([FromBody] WorkTask workTaskInfo)
        {
            return _bllWorkTask.AddWorkTaskInfo(workTaskInfo);
        }

        [HttpPost]
        public object RemoveWorkTaskByKey(string uuid)
        {
            return base.OperationToResult(() =>
               {
                   return _bllWorkTask.RemoveWorkTaskByKey(uuid);
               });
        }

        [HttpGet]
        public List<KeyValuePair<string, Project>> GetWorkTask()
        {
            var projects = _bllProject.GetProject();
            if (projects == null)
            {
                return null;
            }

            var tasks = _bllWorkTask.GetWorkTask();
            if (tasks == null)
            {
                return null;
            }
            List<KeyValuePair<string, Project>> pro = new List<KeyValuePair<string, Project>>();
            var ids = tasks.Select(t => t.Value.PojectId);
            var result = projects.Where(t => ids.Contains(t.Key));
            return result == null ? result.ToList() : null;
        }

        [HttpGet]
        public IEnumerable<KeyValuePair<string, Project>> GetWorkTaskByProject(string projectId)
        {
            var projects = this.GetWorkTask();

            if (projects == null)
            {
                return null;
            }
            return projects.Where(t => t.Key == projectId);
        }

        [HttpGet]
        public KeyValuePair<string, Project> GetWorkTaskByGuid(string guid)
        {
            var projects = this.GetWorkTask();
            if (projects == null)
            {
                return default(KeyValuePair<string, Project>);
            }
            return projects.Where(t => t.Value.Guid == guid).FirstOrDefault();
        }

        [HttpPost]
        public object AddWorkTask([FromBody] WorkTask workTask)
        {
            return base.OperationToResult(() =>
            {
                return _bllWorkTask.AddWorkTask(workTask);
            });
        }
    }
}
