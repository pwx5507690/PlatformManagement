using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using PlatformManagement.Models;
using DataSource;
using PlatformManagement.Common;
using PlatformManagement.Controllers.Bll;

namespace PlatformManagement.Controllers
{
    public class ProjectController : BaseCommonController
    {
        
        public readonly BllProject _bllProject = new BllProject();

        public object AddProject([FromBody] Project project)
        {
            return base.OperationToResult(() =>
             {
                return _bllProject.AddProject(project);
             });
        }

        private object RemoveByKey(string key)
        {
            return base.OperationToResult(() =>
            {
                return _bllProject.RemoveByKey(key);          
           });
        }

        [HttpPost]
        public object Update([FromBody] Project project)
        {
            return base.OperationToResult(() =>
            {
                return _bllProject.Update(project);
            });
        }

        [HttpPost]
        public List<KeyValuePair<string, Project>> GetProject()
        {
            return _bllProject.GetProject();
        }

        [HttpPost]
        public object Delete(Dyna dyna)
        {
            return base.OperationToResult(() =>
            {
                string key = dyna.GetPropertyValue("guid").ToString();

                return _bllProject.RemoveByKey(key);
            });
        }
        [HttpGet]
        public Result<KeyValuePair<string, Project>> GetBageProject()
        {
            return base.GetPage<KeyValuePair<string, Project>>(_bllProject.GetProject());
        }

        [HttpGet]
        public Result<KeyValuePair<string, Project>> GetProjectByName(string name)
        {
            return base.GetPage<KeyValuePair<string, Project>>(_bllProject.GetProject().Where(t => t.Value.Name.Contains(name)));
        }

        [HttpPost]
        public object GetProjectByDate(Dyna dyna)
        {
            return base.GetPage<KeyValuePair<string, Project>>(_bllProject.GetProject().Where(t => t.Value.RunTime >= (int)dyna.GetPropertyValue("date")));
        }

        [HttpPost]
        public List<KeyValuePair<string, Project>> GetProjectByUuid(Dyna dyna)
        {
            return _bllProject.GetProject().Where(t => t.Key.ToString().Contains(dyna.GetPropertyValue("uuid").ToString())).ToList();
        }
    }
}
