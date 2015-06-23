using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Castle.DynamicProxy;
using sweet.framework.Infrastructure.Attr;
using sweet.framework.Infrastructure.Interfaces;
using sweet.framework.Utility.Serialization;

namespace sweet.framework.Infrastructure.Interceptors
{
    public class CacheInterceptor : IInterceptor
    {
        private readonly ICacheProvider _cacheProvider;

        private static readonly Dictionary<string, HashSet<string>> dict = new Dictionary<string, HashSet<string>>();

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
                var expireSecond = cacheAttr.ExpireSecond;
                string publishKey = GetCacheKey(cacheAttr.Publish, parameterInfo, invocation.Arguments);
                string[] subscribeKeys = cacheAttr.Subscribe;

                //Set Subscribe Relation
                Subscribe(keyName, subscribeKeys, invocation);

                //Get Cache Data
                if (false == string.IsNullOrEmpty(keyName))
                {
                    object value = _cacheProvider.Get(keyName);
                    GetCacheData(invocation, keyName, value, expireSecond);
                    return;
                }

                //Publish
                Debug.WriteLine(invocation.TargetType.Name + "." + invocation.MethodInvocationTarget.Name + ": Publish Cache Key");
                Publish(publishKey);
            }

            invocation.Proceed();		//Proceed
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
                    if (false == dict.ContainsKey(item))
                    {
                        dict[item] = new HashSet<string>();
                    }
                    var collection = dict[item];
                    collection.Add(keyName);
                }
            }
        }

        private void Publish(string publishKey)
        {
            if (false == string.IsNullOrEmpty(publishKey))
            {
                if (dict.ContainsKey(publishKey))
                {
                    var subs = dict[publishKey];
                    if (subs != null && subs.Count > 0)
                    {
                        _cacheProvider.Remove(subs.ToArray());
                    }
                }
            }
        }

        private void GetCacheData(IInvocation invocation, string keyName, object value, int expireSecond)
        {
            if (value != null && true)
            {
                Debug.WriteLine(invocation.TargetType.Name + "." + invocation.MethodInvocationTarget.Name + ": Get Cache Data");
                invocation.ReturnValue = value;
            }
            else
            {
                Debug.WriteLine(invocation.TargetType.Name + "." + invocation.MethodInvocationTarget.Name + ": Cache not data");
                invocation.Proceed();
                //Proceed
                var cacheValue = invocation.ReturnValue;
                _cacheProvider.Set(keyName, cacheValue, expireSecond);
            }
        }
    }
}