using Castle.DynamicProxy;
using sweet.framework.Infrastructure.Interceptors;
using sweet.framework.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;

namespace sweet.framework.Infrastructure
{
    public class RemoteInvoker
    {
        private static readonly Dictionary<RuntimeTypeHandle, object> _cache = new Dictionary<RuntimeTypeHandle, object>();
        private readonly ProxyGenerator _proxy = new ProxyGenerator();
        private readonly IInterceptor[] _interceptors;

        protected RemoteInvoker(IInterceptor[] remoteInvokeInterceptor)
        {
            _interceptors = remoteInvokeInterceptor;
        }

        /// <summary>
        /// 返回基于TInvoker拦截器的 远程调用器
        /// </summary>
        /// <typeparam name="TInvoker"></typeparam>
        /// <param name="remoteInvokeInterceptor"></param>
        /// <returns></returns>
        public static RemoteInvoker GetInvoker<TInvoker>(TInvoker remoteInvokeInterceptor)
            where TInvoker : RemoteInvokeInterceptor
        {
            return new RemoteInvoker(new TInvoker[] { remoteInvokeInterceptor });
        }

        /// <summary>
        /// 返回远程调用客户端
        /// </summary>
        /// <typeparam name="TInterface"></typeparam>
        /// <returns></returns>
        public TInterface GetClient<TInterface>()
            where TInterface : class, IService
        {
            var key = typeof(TInterface).TypeHandle;
            if (false == _cache.ContainsKey(key))
            {
                var interfaceClient = _proxy.CreateInterfaceProxyWithoutTarget<TInterface>(_interceptors);
                _cache[key] = interfaceClient;
            }

            var cacheClient = _cache[key] as TInterface;

            return cacheClient;
        }
    }
}