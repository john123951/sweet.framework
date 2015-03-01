using System;

namespace test.Infrastructure
{
	public interface ICacheProvider
	{
		object Get (string key, Type valueType);

		T Get<T> (string key);

		void Set (string key, object value, Type valueType, long second = 20 * 60);

		void Remove (params string[] keys);
	}
}

