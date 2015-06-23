using System.Diagnostics;
using Castle.DynamicProxy;

namespace sweet.framework.Infrastructure.Interceptors
{
    public class TraceInterceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            invocation.Proceed();
            stopWatch.Stop();

            Debug.WriteLine("{0}: {1}ms", invocation.TargetType.Name + "." + invocation.MethodInvocationTarget.Name, stopWatch.ElapsedMilliseconds);
        }
    }
}