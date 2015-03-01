using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Castle.DynamicProxy;
using test.Infrastructure;
using System.Diagnostics;

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
				Subscribe (keyName, subscribeKeys, invocation.Arguments);

				//Get Cache Data
				if (false == string.IsNullOrEmpty (keyName)) {
					object value = CacheProvider.Get (keyName);
					GetCacheData (invocation, keyName, value, expireSecond);
					return;
				}

				//Publish
				Publish (publishKey);
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
			if (subscribeKeys != null && subscribeKeys.Length > 0) {
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
		}

		void Publish (string publishKey)
		{
			if (false == string.IsNullOrEmpty (publishKey)) {
				Console.ForegroundColor = ConsoleColor.Green;
				Debug.WriteLine ("Publish Cache Key");
				Console.ResetColor ();
				if (dict.ContainsKey (publishKey)) {
					var subs = dict [publishKey];
					if (subs != null && subs.Count > 0) {
						CacheProvider.Remove (subs.ToArray ());
					}
				}
			}
		}

		void GetCacheData (IInvocation invocation, string keyName, object value, int expireSecond)
		{
			if (value != null && true) {
				Console.ForegroundColor = ConsoleColor.Green;
				Debug.WriteLine ("Get Cache Data");
				Console.ResetColor ();
				invocation.ReturnValue = value;
			} else {
				Console.ForegroundColor = ConsoleColor.DarkRed;
				Debug.WriteLine ("Cache not data");
				Console.ResetColor ();
				invocation.Proceed ();
				//Proceed
				var cacheValue = invocation.ReturnValue;
				CacheProvider.Set (keyName, cacheValue, expireSecond);
			}
		}
	}
}
