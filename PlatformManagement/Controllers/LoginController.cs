using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using PlatformManagement.Models;

namespace PlatformManagement.Controllers
{
    public class LoginController : BaseCommonController
    {
        [HttpGet]
        public List<Menu> GetMainInitModel()
        {
            return GetMenu();
        }

        private List<Menu> GetMenu()
        {
            List<Menu> menu = Menu.GetMenuConfig;
            menu.AddRange(((List<Menu>)new MenuController().GetMenu()).Where(t => !string.IsNullOrEmpty(t.Url)));
            return menu;
        }
    }

}
