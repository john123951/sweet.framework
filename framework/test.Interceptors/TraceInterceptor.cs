using System;
using Castle.DynamicProxy;
using System.Diagnostics;

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
			Console.ForegroundColor = ConsoleColor.Cyan;
			Debug.WriteLine ("Before : {0}.{1}()", invocation.TargetType.Name, methodInfo.Name);
			Console.ResetColor ();

			invocation.Proceed ();

			Console.ForegroundColor = ConsoleColor.Cyan;
			Debug.WriteLine ("End : {0}.{1}()", invocation.TargetType.Name, methodInfo.Name);
			Console.ResetColor ();
		}

	}
}

