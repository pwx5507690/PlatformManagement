using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using PlatformManagement.Models;
using PlatformManagement.Hubs;
using DataSource;
using PlatformManagement.Common;
using PlatformManagement.Controllers.Bll;


namespace PlatformManagement.Controllers
{
    public class MemberController : BaseCommonController
    {
        private readonly BllMember _bllMember = new BllMember();
        [HttpPost]
        public object AddAccountForAdminPage([FromBody] Account account)
        {
            return base.OperationToResult(() =>
            {
                return _bllMember.AddAccountForAdminPage(account);
            });
        }

        [HttpGet]
        public bool ValidateMail(string mail)
        {
            return _bllMember.ValidateMail(mail);
        }

        [HttpPost]
        public object AddAccount([FromBody] Account account)
        {
            return _bllMember.AddAccount(account);
        }
        [HttpPost]
        public object Login([FromBody] Account account)
        {
            return _bllMember.Login(account, base.RequestBase.UserHostAddress);
        }
        [HttpGet]
        public List<KeyValuePair<string, Account>> GetAccountById(string uuid)
        {
            return _bllMember.GetAccountById(uuid);
        }
        [HttpPost]
        public object RemoveByKey([FromBody] Dyna dyna)
        {
            return base.OperationToResult(() =>
            {
                string guid = dyna.GetPropertyValue("uuid").ToString();
                return _bllMember.RemoveByKey(guid);
            });
        }

        [HttpGet]
        public List<KeyValuePair<string, Account>> GetAllAccount()
        {
            return _bllMember.GetAccount();
        }

        [HttpGet]
        public Result<KeyValuePair<string, Account>> GetAccount()
        {
            return base.GetPage<KeyValuePair<string, Account>>(_bllMember.GetAccount());
        }

        [HttpGet]
        public Result<KeyValuePair<string, Account>> GetAccountByName(string name)
        {
            var accounts = _bllMember.GetAccount().Where(t => t.Value.AccountName.Contains(name));
            return base.GetPage<KeyValuePair<string, Account>>(accounts);
        }
        [HttpPost]
        public object FoundAccount([FromBody] Account account)
        {
            return base.OperationToResult(() =>
            {
                //bool isExist = RedisBase.HashGetAll<KeyValuePair<string, Account>>(MEMBER)
                //    .Any(t => t.Value.AccountName == account.AccountName);
                //if (!isExist)
                //{
                //    //return "该账号不存在";
                //}
                //// Mail mail = new Mail();
                //// mail.SendMessage();
                //return isExist;
                return true;
            });

        }

    }

}
