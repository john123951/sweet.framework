using Enyim.Caching;
using Enyim.Caching.Configuration;
using Enyim.Caching.Memcached;
using sweet.framework.Infrastructure.Config;
using sweet.framework.Infrastructure.Interfaces;
using System;

namespace sweet.framework.CacheProvider
{
    public class MemcachedCacheProvider : ICacheProvider
    {
        public object Get(string key)
        {
            using (var client = GetClient())
            {
                var result = client.Get(key);
                return result;
            }
        }

        public object Get(string key, Type type)
        {
            using (var client = GetClient())
            {
                var result = client.Get(key);
                return result;
            }
        }

        public T Get<T>(string key)
             where T : class
        {
            using (var client = GetClient())
            {
                var result = client.Get<T>(key);
                return result;
            }
        }

        public void Set(string key, object value, long second = 20 * 60)
        {
            using (var client = GetClient())
            {
                var expireTime = DateTime.Now.AddSeconds(second);

                client.Store(StoreMode.Set, key, value, expireTime);
            }
        }

        public void Set<T>(string key, T value, long second = 1200)
        {
            Set(key, value, second);
        }

        public void Remove(params string[] keys)
        {
            if (keys == null || keys.Length <= 0)
            {
                return;
            }

            using (var client = GetClient())
            {
                foreach (var item in keys)
                {
                    client.Remove(item);
                }
            }
        }

        private MemcachedClient GetClient()
        {
            var configuration = new MemcachedClientConfiguration();

            configuration.AddServer(GlobalConfig.MemcacheHost);

            var client = new MemcachedClient(configuration);

            return client;
        }
    }
}