using Castle.DynamicProxy;
using sweet.framework.Infrastructure.Attr;
using sweet.framework.Infrastructure.Interfaces;
using sweet.framework.Utility.Serialization;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace sweet.framework.Infrastructure.Interceptors
{
    /// <summary>
    /// 缓存 拦截器
    /// 请继承此类，重写CanCache()方法
    /// </summary>
    public class CacheInterceptor : IInterceptor
    {
        private readonly ICacheProvider _cacheProvider;

        private static readonly Dictionary<string, HashSet<string>> subscribeDict = new Dictionary<string, HashSet<string>>();

        public CacheInterceptor(ICacheProvider cacheProvider)
        {
            _cacheProvider = cacheProvider;
        }

        public void Intercept(IInvocation invocation)
        {
            var methodInfo = invocation.MethodInvocationTarget;
            var cacheAttr = methodInfo.GetCustomAttributes(true).FirstOrDefault(x => x is CacheAttribute) as CacheAttribute;

            var parameterInfo = invocation.MethodInvocationTarget.GetParameters();

            //Has Cache Attribute
            if (cacheAttr != null)
            {
                var keyName = GetCacheKey(cacheAttr.KeyName, parameterInfo, invocation.Arguments);
                string publishKey = GetCacheKey(cacheAttr.Publish, parameterInfo, invocation.Arguments);
                var expireSecond = cacheAttr.ExpireSecond;
                string[] subscribeKeys = cacheAttr.Subscribe;

                //Set Subscribe Relation
                Subscribe(keyName, subscribeKeys, invocation);

                //Get Cache Data
                if (GetCacheData(invocation, keyName, expireSecond))
                {
                    return;
                }

                //Publish
                if (Publish(publishKey)) { Debug.WriteLine(invocation.TargetType.Name + "." + invocation.MethodInvocationTarget.Name + ": Publish Cache Key"); }
            }

            //else ,Proceed
            invocation.Proceed();
        }

        private static string GetCacheKey(string keyPattern, ParameterInfo[] parameterInfo, object[] param)
        {
            if (string.IsNullOrEmpty(keyPattern))
            {
                return string.Empty;
            }
            var result = keyPattern;

            if (parameterInfo.Any())
            {
                var parmArr = Regex.Matches(result, @"{.+?}");

                foreach (Match parmPattern in parmArr)
                {
                    if (!parmPattern.Success) { continue; }

                    //Get Index
                    int index = 0;
                    for (int i = 0; i < parameterInfo.Length; i++)
                    {
                        var item = parameterInfo[i];
                        var parmValue = parmPattern.Value.Trim('{', '}');

                        if (item.Name == parmValue || parmValue.StartsWith(item.Name + "."))
                        {
                            index = i; break;
                        }
                    }
                    //Get value
                    string value = SerializeObject(parmPattern.Value, param[index]);

                    //Replace value
                    result = result.Replace(parmPattern.Value, value);
                }
            }

            return result;
        }

        private static string SerializeObject(string parmPattern, object parm)
        {
            var arr = parmPattern.Trim('{', '}').Split(new[] { "." }, StringSplitOptions.RemoveEmptyEntries);

            object resultObj = parm;

            if (arr.Length > 1)
            {
                for (int i = 1; i < arr.Length; i++)
                {
                    resultObj = resultObj.GetType().GetProperty(arr[i]).GetValue(resultObj, null);
                }
            }

            return JsonUtility.Serialize(resultObj);
        }

        private static void Subscribe(string keyName, string[] subscribeKeys, IInvocation invocation)
        {
            if (subscribeKeys != null && subscribeKeys.Length > 0)
            {
                for (int i = 0; i < subscribeKeys.Length; i++)
                {
                    var item = subscribeKeys[i];
                    subscribeKeys[i] = GetCacheKey(item, invocation.MethodInvocationTarget.GetParameters(), invocation.Arguments);
                }
                foreach (var item in subscribeKeys)
                {
                    if (false == subscribeDict.ContainsKey(item))
                    {
                        subscribeDict[item] = new HashSet<string>();
                    }
                    subscribeDict[item].Add(keyName);
                }
            }
        }

        private bool Publish(string publishKey)
        {
            if (string.IsNullOrEmpty(publishKey)) { return false; }

            if (subscribeDict.ContainsKey(publishKey))
            {
                var subs = subscribeDict[publishKey];
                if (subs != null && subs.Count > 0)
                {
                    _cacheProvider.Remove(subs.ToArray());
                }
                return true;
            }
            return false;
        }

        private bool GetCacheData(IInvocation invocation, string keyName, int expireSecond)
        {
            if (false == string.IsNullOrEmpty(keyName))
            {
                object value = _cacheProvider.Get(keyName, invocation.Method.ReturnType);

                if (value != null)
                {
                    invocation.ReturnValue = value;
                    Debug.WriteLine(invocation.TargetType.Name + "." + invocation.MethodInvocationTarget.Name + ": Get Cache Data");
                    return true;
                }
            }

            //Proceed
            invocation.Proceed();
            var returnValue = invocation.ReturnValue;

            //Cache
            if (CanCache(returnValue))
            {
                _cacheProvider.Set(keyName, returnValue, expireSecond);
            }

            Debug.WriteLine(invocation.TargetType.Name + "." + invocation.MethodInvocationTarget.Name + ": Cache not data");
            return false;
        }

        protected virtual bool CanCache(object returnValue)
        {
            return returnValue != null;
        }
    }
}