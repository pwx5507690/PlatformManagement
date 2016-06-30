using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataSource;
using PlatformManagement.Models;
using PlatformManagement.Hubs;

namespace PlatformManagement.Controllers.Bll
{

    public class BllMember : BllBase
    {
        public static readonly string MEMBER = "MEMBER";

        public BllMember() : base(MEMBER)
        {

        }

        public List<KeyValuePair<string, Account>> GetAccountById(string uuid)
        {
            return RedisBase.HashGetAll<KeyValuePair<string, Account>>(this._key).Where(t => t.Key == uuid).ToList();
        }

        public bool AddAccountForAdminPage(Account account)
        {
            var item = RedisBase.HashGetAll<KeyValuePair<string, Account>>(this._key);
            if (item != null)
            {
                var accounts = item.Where(t => t.Value.AccountName == account.AccountName);

                if (accounts.Any())
                {
                    var count = item.Where(t => t.Value.AccountName.Contains(account.AccountName)).Count();
                    account.AccountName = account.AccountName + "(" + count + ")";
                }
            }
            account.Id = Guid.NewGuid().ToString();

            if (account.Password == "-1")
            {
                account.Password = account.ConfirmPassword = "123346";
            }

            KeyValuePair<string, Account> model = new KeyValuePair<string, Account>(account.Id, account);

            return RedisBase.HashSet<KeyValuePair<string, Account>>(this._key, model.Key, model);
        }

        public bool ValidateMail(string mail)
        {
            var item = this.GetAccount();
            //RedisBase.HashGetAll<KeyValuePair<string, Account>>(this._key);
            if (item == null)
            {
                return true;
            }
            return item.Where(t => t.Value.Mail == mail).Any();
        }

        public object AddAccount(Account account)
        {
            var item = RedisBase.HashGetAll<KeyValuePair<string, Account>>(this._key);
            if (item != null)
            {
                bool isExist = item.Any(t => t.Value.AccountName == account.AccountName);
                if (isExist)
                {
                    return new
                    {
                        result = false,
                        message = "昵称已经被占用"
                    };

                }
            }
            account.Id = Guid.NewGuid().ToString();
            KeyValuePair<string, Account> model = new KeyValuePair<string, Account>(account.Id, account);

            RedisBase.HashSet<KeyValuePair<string, Account>>(this._key, model.Key, model);
            return new
            {
                result = true,
                message = "success"
            };
        }

        public object Login(Account account, string ipAddress)
        {
            var model = RedisBase.HashGetAll<KeyValuePair<string, Account>>(this._key)
                .Where(t => t.Value.AccountName == account.AccountName);
            //Tuple
            if (!model.Any())
            {
                return new
                {
                    result = false,
                    message = "用户不存在"
                };
            }
            var item = model.FirstOrDefault();
            if (item.Value.Password != account.Password)
            {
                return new
                {
                    result = false,
                    message = "密码错误"
                };
            }
            new BllSession().AddSession(new SessionCache()
            {
                IpAddress = ipAddress,
                Token = item.Value.Id,
                Name = item.Value.UserName
            });

            Chat.SendClientMessage(new Message()
            {
                Timestamp = DateTime.UtcNow,
                Body = "login out",
                MessageType = MessageType.LoginOut
            });

            return new
            {
                result = item
            };
        }

        public bool RemoveByKey(string key)
        {
            return RedisBase.HashRemove(this._key, key);
        }

        public List<KeyValuePair<string, Account>> GetAccount()
        {
            return RedisBase.HashGetAll<KeyValuePair<string, Account>>(this._key);
        }

        public static SessionCache ConvertToSession(Account account)
        {
            return new SessionCache()
            {

                Token = account.Id,
                Name = account.UserName
            };
        }
    }
}