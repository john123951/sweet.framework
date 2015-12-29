using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace sweet.framework.Utility
{
    public static class ReflectionUtility
    {
        /// <summary>
        /// 返回当前调用的方法名
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentMethodName()
        {
            // 这里忽略1层堆栈，也就忽略了当前方法GetMethodName，这样拿到的就正好是外部调用GetMethodName的方法信息
            var method = new StackFrame(1).GetMethod();

            return method.Name;
        }

        /// <summary>
        /// 返回当前调用的属性名
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentPropertyName()
        {
            var method = new StackFrame(1).GetMethod();

            if (method.DeclaringType != null)
            {
                //如果是属性
                var property = (
                    from p in method.DeclaringType.GetProperties(
                        BindingFlags.Instance |
                        BindingFlags.Static |
                        BindingFlags.Public |
                        BindingFlags.NonPublic)
                    where p.GetGetMethod(true) == method || p.GetSetMethod(true) == method
                    select p).FirstOrDefault();
                return property == null ? method.Name : property.Name;
            }

            return string.Empty;
        }
    }
}