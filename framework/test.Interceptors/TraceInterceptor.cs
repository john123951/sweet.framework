using System;
using Castle.DynamicProxy;
using System.Diagnostics;
using test.Utility;

namespace test.Interceptors
{
	public class TraceInterceptor : IInterceptor
	{
		static TraceInterceptor ()
		{
			Debug.Listeners.Clear ();
			Debug.Listeners.Add (new ConsoleTraceListener ());
		}

		public void Intercept (IInvocation invocation)
		{
			var methodInfo = invocation.MethodInvocationTarget;

			ConsoleUtility.WriteLine ("Before : {0}.{1}()", 
			                          ConsoleColor.Cyan, 
			                          invocation.TargetType.Name, methodInfo.Name);

			invocation.Proceed ();

			ConsoleUtility.WriteLine ("End : {0}.{1}()", 
			                          ConsoleColor.Cyan, 
			                          invocation.TargetType.Name, methodInfo.Name);
		}

	}
}

