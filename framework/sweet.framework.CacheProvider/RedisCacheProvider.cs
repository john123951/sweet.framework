using System;
using ServiceStack.Redis;
using sweet.framework.Infrastructure.Config;
using sweet.framework.Infrastructure.Interfaces;

namespace sweet.framework.CacheProvider
{
    public class RedisCacheProvider : ICacheProvider
    {
        private static IRedisClientsManager _clientsManager;

        private static IRedisClient GetClient()
        {
            if (_clientsManager == null)
            {
                _clientsManager = new RedisManagerPool(GlobalConfig.RedisHost);
            }

            return _clientsManager.GetClient();
        }

        public object Get(string key)
        {
            using (var client = GetClient())
            {
                var result = client.Get<object>(key);

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

                client.Set<object>(key, value, expireTime);
            }
        }

        public void Set<T>(string key, T value, long second = 1200)
        {
            using (var client = GetClient())
            {
                var expireTime = DateTime.Now.AddSeconds(second);

                client.Set<T>(key, value, expireTime);
            }
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
    }
}