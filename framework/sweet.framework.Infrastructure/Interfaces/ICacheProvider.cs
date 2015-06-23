using System;

namespace sweet.framework.Infrastructure.Interfaces
{
    public interface ICacheProvider
    {
        object Get(string key);

        object Get(string key, Type type);

        T Get<T>(string key) where T : class;

        void Set(string key, object value, long second = 20 * 60);

        void Set<T>(string key, T value, long second = 20 * 60);

        void Remove(params string[] keys);
    }
}