using Castle.DynamicProxy;
using System;
using System.Diagnostics;

namespace sweet.framework.Infrastructure.Interceptors
{
    /// <summary>
    /// 容错 拦截器
    /// </summary>
    public class TryCatchInterceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            try
            {
                invocation.Proceed();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}