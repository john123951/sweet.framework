using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sweet.framework.Utility.Extention
{
    public static class DateTimeExtensions
    {
        /// <summary>
        /// 转换为 Unix 时间截
        /// 自1970.1.1的秒数
        /// </summary>
        public static double ToTimestamp(this DateTime dateTime)
        {
            var epoch2 = (dateTime.ToUniversalTime() - new DateTime(1970, 1, 1)).TotalSeconds;

            return epoch2;
        }

        /// <summary>
        /// 将毫秒数转换为本地时间
        /// </summary>
        /// <param name="utc"></param>
        /// <returns></returns>
        public static DateTime ConvertIntDatetime(double utc)
        {
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            startTime = startTime.AddSeconds(utc);

            return startTime;
        }
    }
}