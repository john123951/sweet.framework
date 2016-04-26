using Castle.DynamicProxy;
using sweet.framework.Infrastructure.Model;
using sweet.framework.Utility;
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
                invocation.ReturnValue = ReturnValue(invocation.Method.ReturnType, ex);
                LogUtility.GetInstance().WriteLog(invocation.TargetType.Name, LogUtility.LogType.Error, "tryCatch", ex);
            }
        }

        protected virtual object ReturnValue(Type returnType, Exception ex)
        {
            return null;
        }
    }

    public class HanderTryCatchInterceptor : TryCatchInterceptor
    {
        protected override object ReturnValue(Type returnType, Exception ex)
        {
            var handler = Activator.CreateInstance(returnType) as ResultHandler;

            if (handler != null)
            {
                handler.Message = ex.Message;
            }

            return handler;
        }
    }
}