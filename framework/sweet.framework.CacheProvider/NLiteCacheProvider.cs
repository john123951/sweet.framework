﻿using NLite.Cache;
using sweet.framework.Infrastructure.Interfaces;
using System;

namespace sweet.framework.CacheProvider
{
    public class NLiteCacheProvider : ICacheProvider
    {
        private readonly ICache _cache = CacheManager.Cache;

        public object Get(string key)
        {
            return _cache[key];
        }

        public object Get(string key, Type type)
        {
            return _cache[key];
        }

        public T Get<T>(string key) where T : class
        {
            return _cache[key] as T;
        }

        public void Set(string key, object value, long second)
        {
            _cache.Insert(key, value, DateTime.Now.AddSeconds(second), TimeSpan.Zero);
        }

        public void Set<T>(string key, T value, long second = 1200)
        {
            Set(key, (object)value, second);
        }

        public void Remove(params string[] keys)
        {
            if (keys == null) { return; }

            foreach (var key in keys)
            {
                if (_cache.ContainsKey(key))
                {
                    _cache.Remove(key);
                }
            }
        }
    }
}