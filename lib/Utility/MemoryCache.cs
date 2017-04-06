using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using Microsoft.Practices.EnterpriseLibrary.Caching.Expirations;

namespace com.ccfw.Utility
{
    public class MemoryCache
    {
        public static object lockobj = new object();
        private static readonly ICacheManager cacheManager = CacheFactory.GetCacheManager();
        public static T Get<T>(string key)
        {
            //return cacheManager.GetData(key) as  T;
            var obj = cacheManager.GetData(key);
            if (obj == null)
            {
                return default(T);
            }
            else
            {
                return (T)obj;
            }

        }

        public static void Set(string key, object data, DateTime cacheTime)
        {
            cacheManager.Add(key, data, CacheItemPriority.High, null, new AbsoluteTime(cacheTime));
        }

        public static bool IsSet(string key)
        {
            return true;
        }

        public static void Remove(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                lock (lockobj)
                {
                    cacheManager.Flush();
                }
            }
            else if (cacheManager.Contains(key))
            {
                lock (lockobj)
                {
                    cacheManager.Remove(key);
                }
            }
        }




        public static void RemoveByPattern(string pattern)
        {

        }
    }
}
