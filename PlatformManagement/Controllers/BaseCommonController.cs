using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace PlatformManagement.Controllers
{
    public class Result<T>
    {
        public int currentPage { get; set; }
        public IEnumerable<T> result { get; set; }
        public int pager { get; set; }
    }
    public class BaseCommonController : ApiController
    {
        protected Result<T> GetPage<T>(IEnumerable<T> list)
        {
            if (list == null)
            {
                return new Result<T>();
            }
            int pageSize = Convert.ToInt32(this.RequestBase["pageSize"]);
            int currentPage = Convert.ToInt32(this.RequestBase["currentPage"]);

            Result<T> r = new Result<T>();
            r.result = list.Skip(pageSize * currentPage).Take(pageSize);
            r.currentPage = currentPage + 1;
            int count = list.Count();
            r.pager = count % pageSize == 0 ? count / pageSize : (count / pageSize) + 1;
            return r;
        }
        protected System.Web.HttpRequestBase RequestBase
        {
            get
            {
                return ((HttpContextBase)Request.Properties["MS_HttpContext"]).Request;
            }
        }
        protected object OperationToResult(Func<bool> func)
        {
            try
            {
                bool mes = func();
                return new
                {
                    result = mes ? true : false,
                    message = mes ? "success" : "error"
                };
            }
            catch (Exception e)
            {
                return new
                {
                    result = false,
                    message = "error"
                };
            }
        }
    }
}
