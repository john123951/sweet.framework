using Castle.DynamicProxy;
using sweet.framework.Infrastructure.Attr;
using sweet.framework.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

/* =======================================================================
* 创建时间：2016/1/23 10:36:53
* 作者：sweet
* Framework: 4.5
* ========================================================================
*/

namespace sweet.framework.Infrastructure.Interceptors
{
    /// <summary>
    /// 事务拦截器
    /// </summary>
    public abstract class TransactionInterceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            var methodInfo = invocation.MethodInvocationTarget;
            var cacheAttr = methodInfo.GetCustomAttributes(true).FirstOrDefault(x => x is TransactionAttribute) as TransactionAttribute;

            if (cacheAttr == null)
            {
                invocation.Proceed();

                return;
            }
            using (var transaction = new TransactionScope())
            {
                //transaction.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
                //transaction.Timeout = new TimeSpan(0, 2, 0);

                try
                {
                    invocation.Proceed();

                    if (IsComplete(invocation.ReturnValue))
                    {
                        transaction.Complete();
                        return;
                    }
                }
                catch (Exception ex)
                {
                    LogUtility.GetInstance().Error("Transaction Error: " + ex.Message);
                }

                LogUtility.GetInstance().Info("Transaction Rollback......");
            }
        }

        protected abstract bool IsComplete(object returnValue);
    }
}