using System;
using System.Reflection;
using System.Text;

namespace sweet.framework.Utility.Extention
{
    public static class ObjectExtention
    {
        /// <summary>
        /// 遍历对象的公有属性，生成字符串
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="split"></param>
        /// <returns></returns>
        public static string GetFeildString(this object instance, string split = "\r\n")
        {
            if (instance == null) { return String.Empty; }

            StringBuilder sbInfo = new StringBuilder();

            var props = instance.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
            foreach (var propertyInfo in props)
            {
                sbInfo.AppendFormat("{0}:{1}", propertyInfo.Name, propertyInfo.GetValue(instance, null));
                sbInfo.Append(split);
            }

            return sbInfo.ToString(0, Math.Max(0, sbInfo.Length - split.Length));
        }

        /// <summary>
        /// 实例为null
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="argumentName"></param>
        public static void ThrowIfNull(this object instance, string argumentName)
        {
            if (instance == null) throw new ArgumentNullException(argumentName);
        }

        /// <summary>
        /// 判断对象各个公有属性是否为空
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="message"></param>
        public static void ThrowIfPropertyIsEmpty(this object instance, string message)
        {
            instance.ThrowIfNull(message);

            Type type = instance.GetType();
            var props = type.GetProperties();

            foreach (var propertyInfo in props)
            {
                if (propertyInfo.PropertyType == typeof(int) || propertyInfo.PropertyType == typeof(int?))
                {
                    propertyInfo.ThrowIfPropertyIsEmpty(x => Convert.ToInt32((object)x) == default(int), instance, message);
                }
                else if (propertyInfo.PropertyType == typeof(string))
                {
                    propertyInfo.ThrowIfPropertyIsEmpty(x => String.IsNullOrEmpty(x.ToString()), instance, message);
                }
                else if (propertyInfo.PropertyType == typeof(DateTime) || propertyInfo.PropertyType == typeof(DateTime?))
                {
                    propertyInfo.ThrowIfPropertyIsEmpty(x => Convert.ToDateTime((object)x) == default(DateTime), instance, message);
                }
                else if (propertyInfo.PropertyType == typeof(short) || propertyInfo.PropertyType == typeof(short?))
                {
                    propertyInfo.ThrowIfPropertyIsEmpty(x => Convert.ToInt16((object)x) == default(short), instance, message);
                }
                else if (propertyInfo.PropertyType == typeof(decimal) || propertyInfo.PropertyType == typeof(decimal?))
                {
                    propertyInfo.ThrowIfPropertyIsEmpty(x => Convert.ToDecimal((object)x) == default(decimal), instance, message);
                }
                else if (propertyInfo.PropertyType == typeof(bool?))
                {
                    propertyInfo.ThrowIfPropertyIsEmpty(x => false, instance, message);
                }
                //else if (propertyInfo.PropertyType == typeof(bool) || propertyInfo.PropertyType == typeof(bool?))
                //{
                //    propertyInfo.PropertyValueIsNotEmpty(x => Convert.ToBoolean(x) == default(bool), instance);
                //}
                else if (propertyInfo.PropertyType == typeof(byte) || propertyInfo.PropertyType == typeof(byte?))
                {
                    propertyInfo.ThrowIfPropertyIsEmpty(x => Convert.ToByte((object)x) == default(byte), instance, message);
                }
                else if (propertyInfo.PropertyType == typeof(long) || propertyInfo.PropertyType == typeof(long?))
                {
                    propertyInfo.ThrowIfPropertyIsEmpty(x => Convert.ToInt64((object)x) == default(long), instance, message);
                }
                else
                {
                    throw new NotImplementedException("ReflectionExtensions.AssertAllPropertyAreNotEmpty(...) does not yet support this data type: " + propertyInfo.PropertyType);
                }
            }
        }

        private static void ThrowIfPropertyIsEmpty(this PropertyInfo propertyInfo, Func<object, bool> isEmpty, object instance, string message)
        {
            var value = propertyInfo.GetValue(instance, null);

            if (value == null)
            {
                throw new ArgumentException(message + ": " + propertyInfo.ReflectedType + " -- " + propertyInfo.Name + " is null");
            }

            if (isEmpty != null && isEmpty(value))
            {
                throw new ArgumentException(message + ": " + propertyInfo.ReflectedType + " -- " + propertyInfo.Name + " is empty");
            }
        }
    }
}