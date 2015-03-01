using System;
using System.Linq;
using Castle.DynamicProxy;
using System.Diagnostics;

namespace test.Interceptors
{
	public class LogInterceptor : IInterceptor
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

