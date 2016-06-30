using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataSource;
using PlatformManagement.Hubs;
using PlatformManagement.Models;

namespace PlatformManagement.Controllers.Bll
{
    public class BllSession
    {
        // min
        internal static readonly int TIMER = 10;
        public string AddSession(SessionCache sessionCache)
        {
            //SessionCache sCache = 
            //    RedisBase.Hash_Get<SessionCache>(sessionCache.Token, sessionCache.Token);
            dynamic result = this.IsExisted(sessionCache.Token);

            if (result.isExisted)
            {
                RedisBase.HashSet<SessionCache>(sessionCache.Token, sessionCache.Token, sessionCache);
                RedisBase.HashSetExpire(sessionCache.Token, DateTime.Now.AddMinutes(TIMER));
                return null;
            }
            else
            {
                var sCache = result.cache;
                if (sCache.IpAddress == sessionCache.IpAddress)
                {
                    RedisBase.HashSetExpire(sessionCache.Token, DateTime.Now.AddMinutes(TIMER));
                    return null;
                }
                RedisBase.HashRemove(sCache.Token, sCache.Token);
                RedisBase.HashSet<SessionCache>(sessionCache.Token, sessionCache.Token, sessionCache);
                RedisBase.HashSetExpire(sessionCache.Token, DateTime.Now.AddMinutes(TIMER));
               
                return sCache.Token;
            }
        }

        public object IsExisted(string token)
        {
            SessionCache sessionCache =
               RedisBase.HashGet<SessionCache>(token, token);
            return new
            {
                isExisted = sessionCache == null,
                cache = sessionCache
            };
        }
    }
}