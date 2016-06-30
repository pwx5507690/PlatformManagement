using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataSource
{
    public class RedisBase
    {

        private static string RedisRederPath = System.Configuration.ConfigurationSettings.AppSettings["RedisReaderPath"];

        private static string RedisWriterPath = System.Configuration.ConfigurationSettings.AppSettings["RedisWriterPath"];
        //10.0.18.8:6379
        private static PooledRedisClientManager prcm = CreateManager(new string[] { RedisRederPath }, new string[] { RedisWriterPath });
        private static PooledRedisClientManager CreateManager(string[] readWriteHosts, string[] readOnlyHosts)
        {
            // 支持读写分离，均衡负载 
            return new PooledRedisClientManager(readWriteHosts, readOnlyHosts, new RedisClientManagerConfig
            {
                MaxWritePoolSize = 5, // “写”链接池链接数 
                MaxReadPoolSize = 5, // “读”链接池链接数 
                AutoStart = true,
            });
        }
        public static bool HashExist<T>(string key, string dataKey)
        {
            using (IRedisClient redis = prcm.GetClient())
            {
                return redis.HashContainsEntry(key, dataKey);
            }
        }
   
        public static bool HashSet<T>(string key, string dataKey, T t, bool synchronous = true)
        {
            using (IRedisClient redis = prcm.GetClient())
            {
                string value = ServiceStack.Text.JsonSerializer.SerializeToString<T>(t);
                bool isSave = redis.SetEntryInHash(key, dataKey, value);
                if (synchronous)
                {
                    redis.Save();
                }
                return isSave;
            }
        }
       
        public static bool HashRemove(string key, string dataKey, bool synchronous = true)
        {
            using (IRedisClient redis = prcm.GetClient())
            {
                bool isSave = redis.RemoveEntryFromHash(key, dataKey);
                if (synchronous)
                {
                    redis.Save();
                }
                return isSave;
            }
        }
      
        public static bool HashRemove(string key, bool synchronous)
        {
            using (IRedisClient redis = prcm.GetClient())
            {
                bool isSave = redis.Remove(key);
                if (synchronous)
                {
                    redis.Save();
                }
                return isSave;
            }
        }
      
        public static T HashGet<T>(string key, string dataKey)
        {
            using (IRedisClient redis = prcm.GetClient())
            {
                string value = redis.GetValueFromHash(key, dataKey);
                if (string.IsNullOrEmpty(value))
                {
                    return default(T);
                }
                return ServiceStack.Text.JsonSerializer.DeserializeFromString<T>(value);
            }
        }
      
        public static List<T> HashGetAll<T>(string key)
        {
            using (IRedisClient redis = prcm.GetClient())
            {
                var list = redis.GetHashValues(key);
                if (list != null && list.Count > 0)
                {
                    List<T> result = new List<T>();
                    foreach (var item in list)
                    {
                        var value = ServiceStack.Text.JsonSerializer.DeserializeFromString<T>(item);
                        result.Add(value);
                    }
                    return result;
                }
                return null;
            }
        }
      
        public static void HashSetExpire(string key, DateTime datetime)
        {
            using (IRedisClient redis = prcm.GetClient())
            {
                redis.ExpireEntryAt(key, datetime);
            }
        }
    }
}