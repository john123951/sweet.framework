using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Castle.DynamicProxy;
using test.Infrastructure;
using System.Diagnostics;
using test.Utility;

namespace test.Interceptors
{
	public class CacheInterceptor : IInterceptor
	{
		public ICacheProvider CacheProvider { get; set; }

		static Dictionary<string ,HashSet<string>> dict = new Dictionary<string,HashSet<string>> ();

		public void Intercept (IInvocation invocation)
		{
			var methodInfo = invocation.MethodInvocationTarget;
			var cacheAttr = methodInfo.GetCustomAttributes (true).FirstOrDefault (x => x is CacheAttribute) as CacheAttribute;

			//Has Cache Attribute
			if (cacheAttr != null) {
				var keyName = GetCacheKey (cacheAttr.KeyName, invocation.Arguments);
				var expireSecond = cacheAttr.ExpireSecond;
				string publishKey = GetCacheKey (cacheAttr.Publish, invocation.Arguments);
				string[] subscribeKeys = cacheAttr.Subscribe;

				//Set Subscribe Relation
				if (subscribeKeys != null && subscribeKeys.Length > 0) {
					Subscribe (keyName, subscribeKeys, invocation.Arguments);
				}

				//Get Cache Data
				if (false == string.IsNullOrEmpty (keyName)) {
					GetCacheData (invocation, keyName, expireSecond);
					return;
				}

				//Publish
				if (false == string.IsNullOrEmpty (publishKey)) {
					Publish (publishKey);
				}
			}

			invocation.Proceed ();		//Proceed
		}

		static string GetCacheKey (string keyPattern, object[] param)
		{
			if (string.IsNullOrEmpty (keyPattern)) {
				return string.Empty;
			}
			var result = string.Format (keyPattern, param);

			return result;
		}

		static void Subscribe (string keyName, string[] subscribeKeys, object[] param)
		{
			for (int i = 0; i < subscribeKeys.Length; i++) {
				var item = subscribeKeys [i];
				subscribeKeys [i] = GetCacheKey (item, param);
			}
			foreach (var item in subscribeKeys) {
				if (false == dict.ContainsKey (item)) {
					dict [item] = new HashSet<string> ();
				}
				var collection = dict [item];
				collection.Add (keyName);
			}
		}

		void Publish (string publishKey)
		{
			ConsoleUtility.WriteLine ("Publish Cache Key", ConsoleColor.Green);

			if (dict.ContainsKey (publishKey)) {
				var subs = dict [publishKey];
				if (subs != null && subs.Count > 0) {
					CacheProvider.Remove (subs.ToArray ());
				}
			}

		}

		void GetCacheData (IInvocation invocation, string keyName, int expireSecond)
		{
			var methodInfo = invocation.MethodInvocationTarget;

			object value = CacheProvider.Get (keyName, methodInfo.ReturnType);

			if (value != null && true) {
				ConsoleUtility.WriteLine ("Get Cache Data", ConsoleColor.Green);

				invocation.ReturnValue = value;
				return;
			} else {
				ConsoleUtility.WriteLine ("Cache not data", ConsoleColor.DarkRed);

				invocation.Proceed ();
				//Proceed
				var cacheValue = invocation.ReturnValue;
				CacheProvider.Set (keyName, cacheValue, methodInfo.ReturnType, expireSecond);
			}
		}
	}
}
