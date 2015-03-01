using System;
using Enyim.Caching;
using Enyim.Caching.Memcached;
using Enyim.Caching.Configuration;

namespace test.Infrastructure
{
	public class MemcachedCacheProvider : ICacheProvider
	{
		public object Get (string key)
		{
			var client = GetClient ();

			var result = client.Get (key);
			return result;
		}

		public T Get<T> (string key)
		{
			var client = GetClient ();

			var result = client.Get<T> (key);
			return result;
		}

		public void Set (string key, object value, long second = 20 * 60)
		{
			var client = GetClient ();
			var expireTime = DateTime.Now.AddSeconds (second);

			client.Store (StoreMode.Set, key, value, expireTime);
		}

		public void Remove (params string[] keys)
		{
			if (keys == null || keys.Length <= 0) {
				return;
			}

			var client = GetClient ();
			foreach (var item in keys) {
				client.Remove (item);				
			}
		}

		MemcachedClient GetClient ()
		{
			var configuration = new MemcachedClientConfiguration ();

			configuration.AddServer (MemcacheConfig.HostAddress);

			var client = new MemcachedClient (configuration);

			return client;
		}
	}
}

