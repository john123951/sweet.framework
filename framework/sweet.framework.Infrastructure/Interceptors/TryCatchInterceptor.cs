using System;
using System.Diagnostics;
using Castle.DynamicProxy;

namespace sweet.framework.Infrastructure.Interceptors
{
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

