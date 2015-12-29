using Castle.DynamicProxy;
using sweet.framework.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/* =======================================================================
* 创建时间：2015/12/9 16:22:42
* 作者：sweet
* Framework: 4.5
* ========================================================================
*/

namespace sweet.framework.Infrastructure.Interceptors
{
    /// <summary>
    /// 自动调用通知接口
    /// </summary>
    public class InvokeInterceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            var methodInfo = invocation.MethodInvocationTarget;

            string methodName = invocation.Method.Name;
            LogUtility.GetInstance().Debug("InvokeInterceptor: " + methodName);

            invocation.Proceed();

            if (methodName.StartsWith("set_"))
            {
                string propertyName = methodName.Substring(4);

                invocation.TargetType.InvokeMember("RaisePropertyChanged",
                    System.Reflection.BindingFlags.InvokeMethod, null,
                    invocation.InvocationTarget,
                    new object[] { propertyName }
                    );
                LogUtility.GetInstance().Debug("==== InvokeInterceptor: Raise Property = {0} ====", propertyName);
            }
        }
    }
}