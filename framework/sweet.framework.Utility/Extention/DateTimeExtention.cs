using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sweet.framework.Utility.Extention
{
    public static class DateTimeExtention
    {
        /// <summary>
        /// 转换为 Unix 时间截
        /// 自1970.1.1的秒数
        /// </summary>
        public static double GetTimestamp(this DateTime dateTime)
        {
            var epoch2 = (dateTime.ToUniversalTime() - new DateTime(1970, 1, 1)).TotalSeconds;

            return epoch2;
        }
    }
}
