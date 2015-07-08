using Castle.DynamicProxy;
using sweet.framework.Infrastructure.Attr;
using System.Linq;

namespace sweet.framework.Infrastructure.Interceptors
{
    /// <summary>
    /// 重试机制 拦截器
    /// </summary>
    public abstract class RetryInterceptor : IInterceptor
    {
        //private readonly ILog logger = log4net.LogManager.GetLogger("RetryLog");

        public void Intercept(IInvocation invocation)
        {
            var methodInfo = invocation.MethodInvocationTarget;
            var retryAttr = methodInfo.GetCustomAttributes(true).FirstOrDefault(x => x is RetryAttribute) as RetryAttribute;

            invocation.Proceed();

            //Has Retry Attribute
            if (retryAttr != null)
            {
                int retryCount = retryAttr.RetryCount;
                int currentCount = 0;

                while (!IsSuccess(invocation.ReturnValue) && currentCount < retryCount)
                {
                    currentCount++;
                    invocation.Proceed();
                }

                if (currentCount > 0)
                {
                    //Log
                    //logger.Info(invocation.TargetType.Name + "." + invocation.Method.Name + ": RetryCount = " + currentCount);
                }
            }
        }

        protected abstract bool IsSuccess(object returnValue);
    }

    //public class WynRetryInterceptor : RetryInterceptor
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