using Castle.DynamicProxy;
using System.Diagnostics;

namespace sweet.framework.Infrastructure.Interceptors
{
    /// <summary>
    /// 远程过程调用RPC 拦截器
    /// </summary>
    public abstract class RemoteInvokeInterceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            Debug.WriteLine("RemoteInvoke: {0}", invocation.TargetType.Name + "." + invocation.MethodInvocationTarget.Name);
            //Debug.WriteLine("Data: " + JsonUtility.Serialize(invocation.Arguments));

            //RPC
            var resultObject = RemoteInvoke(invocation);

            //为方法返回值赋值
            invocation.ReturnValue = resultObject;
        }

        /// <summary>
        /// 进行远程调用，并返回函数执行结果
        /// </summary>
        /// <param name="invocation"></param>
        /// <returns></returns>
        protected abstract object RemoteInvoke(IInvocation invocation);
    }
}