using System;
using System.Diagnostics;
using Castle.DynamicProxy;

namespace sweet.framework.Infrastructure.Interceptors
{
    /// <summary>
    /// 容错 拦截器
    /// </summary>
	public class TryCatchInterceptor : IInterceptor
	{
		public void Intercept (IInvocation invocation)
		{
			try {
				invocation.Proceed ();
				
			} catch (Exception ex) {
				Debug.WriteLine (ex.Message);
			}
		}
	}
}

