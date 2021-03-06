﻿using System;

namespace sweet.framework.Infrastructure.Attr
{
    /// <summary>
    /// 标识启用重试机制
    /// </summary>
    public sealed class RetryAttribute : Attribute
    {
        /// <summary>
        /// 重试次数
        /// </summary>
        public int RetryCount { get; set; }

        /// <summary>
        /// 重试间隔(毫秒)
        /// </summary>
        public int Interval { get; set; }

        public RetryAttribute()
        {
            RetryCount = 1;
        }
    }
}