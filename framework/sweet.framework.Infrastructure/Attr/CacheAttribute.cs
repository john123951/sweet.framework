using System;

namespace sweet.framework.Infrastructure.Attr
{
    public sealed class CacheAttribute : Attribute
    {
        public CacheAttribute()
        {
            ExpireSecond = 20 * 60;
        }

        public CacheAttribute(string keyName)
            : this()
        {
            this.KeyName = keyName;
        }

        public CacheAttribute(string keyName, string[] subscribeKeys = null)
            : this()
        {
            this.KeyName = keyName;
            this.Subscribe = subscribeKeys;
        }

        /// <summary>
        /// Cache Key
        /// 使用方法：GetUserInfo{request}
        ///           GetUserInfo{request.pageindex}
        /// </summary>
        public string KeyName { get; set; }

        /// <summary>
        /// 超时时间（秒）
        /// </summary>
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