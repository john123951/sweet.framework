using System;
using ServiceStack.Redis;
using test.Utility;

namespace test.Infrastructure
{
	public class RedisCacheProvider: ICacheProvider
	{
		public object Get (string key, Type valueType)
		{
			var client = GetClient ();

			var strValue = client.Get<string> (key);


			var result = JsonUtility.Deserialize (strValue, valueType);
			return result;
		}

		public T Get<T> (string key)
		{
			var client = GetClient ();

			var result = client.Get<T> (key);
			return result;
		}

		public void Set (string key, object value, Type valueType, long second = 20 * 60)
		{
			var client = GetClient ();
			var expireTime = DateTime.Now.AddSeconds (second);

			var strValue = JsonUtility.Serialize (value);

			client.Set<object> (key, strValue, expireTime);
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

		IRedisClientsManager GetClientsManager ()
		{
			var clientManager = new RedisManagerPool (GlobalConfig.RedisHost);

			return clientManager;
		}


		IRedisClient GetClient ()
		{
			var clientManager = GetClientsManager ();

			return clientManager.GetClient ();
		}

	}
}

