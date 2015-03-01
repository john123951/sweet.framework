using System;
using System.Collections.Generic;

namespace test.Infrastructure
{
	public sealed class CacheAttribute : Attribute
	{
		public CacheAttribute ()
		{
			ExpireSecond = 20 * 60;
		}

		public CacheAttribute (string publishKey) : this ()
		{
			this.Publish = publishKey;
		}

		public CacheAttribute (string keyName, string[] subscribeKeys = null) : this ()
		{
			this.KeyName = keyName;
			this.Subscribe = subscribeKeys;
		}

		public string KeyName { get; set; }

		public int ExpireSecond { get; set; }

		/// <summary>
		/// 订阅Key
		/// </summary>
		/// <value>The subscribe.</value>
		public string[] Subscribe { get; set; }

		/// <summary>
		/// 发布Key
		/// </summary>
		/// <value>The publish.</value>
		public string Publish { get; set; }
	}
}

