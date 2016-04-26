using Castle.DynamicProxy;
using sweet.framework.Utility;
using sweet.framework.Utility.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/* =======================================================================
* 创建时间：2016/4/26 11:05:26
* 作者：sweet
* Framework: 4.5
* ========================================================================
*/

namespace sweet.framework.Infrastructure.Interceptors
{
    /// <summary>
    /// 记录接口错误日志
    /// </summary>
    public abstract class RequestLogInterceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            invocation.Proceed();

            var response = invocation.ReturnValue;

            if (!IsSuccess(response))
            {
                var strRequest = JsonUtility.Serialize(invocation.Arguments);
                var strResponse = JsonUtility.Serialize(response);

                var msg = string.Format("Request: {0}\r\nResponse: {1}", strRequest, strResponse);

                LogUtility.GetInstance().WriteLog("WynLog", LogUtility.LogType.Error, msg);
            }
        }

        protected abstract bool IsSuccess(object returnValue);
    }

    //public class WynRetryInterceptor : RequestLogInterceptor
    //{
    //    protected override bool IsSuccess(object returnValue)
    //    {
    //        var response = returnValue as WynResponse;
    //        if (response != null)
    //        {
    //            return !response.IsError;
    //        }

    //        return false;
    //    }
    //}
}