using System;

namespace test.Infrastructure
{
	public interface ICacheProvider
	{
		object Get (string key);

		T Get<T> (string key);

		void Set (string key, object value, long second = 20 * 60);

		void Remove (params string[] keys);
	}
}

