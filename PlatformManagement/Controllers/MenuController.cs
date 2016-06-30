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
    public class MenuController : ApiController
    {
        public static readonly string SAVA_KEY = "MENU_KEY";
        [HttpPost]
        public object AddMenu([FromBody] Menu value)
        {
            return null;
            //try
            //{
            //    RedisBase.List_Add<Menu>(SAVA_KEY, value);
            //    return new
            //    {
            //        result = true,
            //        message = "success"
            //    };
            //}
            //catch (Exception e)
            //{
            //    return new
            //    {
            //        result = false,
            //        message = e.Message
            //    };
            //}
        }

        [HttpPost]
        public object GetMenu()
        {
            return null;
         //   return RedisBase.List_GetList<Menu>(SAVA_KEY); 
        }
    }
}
