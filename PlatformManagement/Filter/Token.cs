using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using PlatformManagement.Controllers;
using WebGrease;
using PlatformManagement.Controllers.Bll;
using System.Web.Http;

namespace PlatformManagement.Filter
{
    public class Token : ActionFilterAttribute
    {

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var anonymousAction = actionContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>();
            if (!anonymousAction.Any())
            {
                // 验证token
                var token = TokenVerification(actionContext);
            }

            base.OnActionExecuting(actionContext);
        }

        protected virtual string TokenVerification(HttpActionContext actionContext)
        {
            // 获取token
            var token = GetToken(actionContext.ActionArguments, actionContext.Request.Method);

            BllSession bllSession = new BllSession();

            dynamic result = bllSession.IsExisted(token);

            if (!result["isExisted"])
            {
                throw new UserLoginException("Token已失效，请重新登陆!");
            }

            return token;
        }

        private string GetToken(Dictionary<string, object> actionArguments, HttpMethod type)
        {
            var token = "";

            if (type == HttpMethod.Post)
            {
                foreach (var value in actionArguments.Values)
                {
                    token = value.GetType().GetProperty("UserToken") == null
                        ? GetToken(actionArguments, HttpMethod.Get)
                        : value.GetType().GetProperty("UserToken").GetValue(value).ToString();
                }
            }
            else if (type == HttpMethod.Get)
            {
                if (!actionArguments.ContainsKey("UserToken"))
                {
                    throw new Exception("未附带token!");
                }

                if (actionArguments["UserToken"] != null)
                {
                    token = actionArguments["UserToken"].ToString();
                }
                else
                {
                    throw new Exception("token不能为空!");
                }
            }
            else
            {
                throw new Exception("暂未开放其它访问方式!");
            }

            return token;
        }
    }
}