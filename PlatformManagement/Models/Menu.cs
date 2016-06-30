using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PlatformManagement.Common;

namespace PlatformManagement.Models
{
    public class Menu : BaseEnity
    {
        public static List<Menu> GetMenuConfig
        {
            get
            {
                return (List<Menu>)new List<Menu>()
                {
                    new Menu()
                    {
                        Title ="项目工程",
                        Url = "#/project",
                        Icon = "am-icon-wpforms",
                        IsUse = true 
                    },
                    new Menu()
                    {
                        Title ="团队",
                        Url = "#/account",
                        Icon = "am-icon-users",
                        IsUse = true
                    },
                    new Menu()
                    {
                        Title ="任务",
                        Url = "#/task",
                        Icon = "am-icon-tasks",
                        IsUse = true
                    },
                    new Menu()
                    {
                        Title ="测试",
                        Url = "#/test",
                        Icon = "am-icon-bug",
                        IsUse = true
                    },
                    new Menu()
                    {
                        Title ="应用管理",
                        Url = "#/manager",
                        Icon = "am-icon-home",
                        IsUse = true
                    }
                }.ForIn(t =>
                {
                    t.SetGuid();
                });
            }
        }
        public string Title { get; set; }
        public string Url { get; set; }
        public int Parent { get; set; }
        public bool IsUse { get; set; }
        public string Icon { get; set; }
    }
}