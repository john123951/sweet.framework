﻿using System;
using System.Reflection;

namespace sweet.framework.Utility.Extention
{
    public static class ReflectionExtensions
    {
        /// <summary>
        /// 获取所有公有属性
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static PropertyInfo[] GetPublicInstanceProperties(this Type type)
        {
            return type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField | BindingFlags.SetField);
        }

        /// <summary>
        /// 为公有属性设置值
        /// </summary>
        /// <param name="type"></param>
        /// <param name="propertyName"></param>
        /// <param name="value">值</param>
        /// <param name="instance"></param>
        public static void SetPropertyValueFromString(this Type type, string propertyName, string value, object instance)
        {
            var property = type.GetProperty(propertyName);

            if (property.PropertyType == typeof(int) || property.PropertyType == typeof(int?))
            {
                property.SetPropertyValueFromString(Convert.ToInt32, value, instance);
            }
            else if (property.PropertyType == typeof(string))
            {
                property.SetPropertyValueFromString(Convert.ToString, value, instance);
            }
            else if (property.PropertyType == typeof(DateTime) || property.PropertyType == typeof(DateTime?))
            {
                property.SetPropertyValueFromString(Convert.ToDateTime, value, instance);
            }
            else if (property.PropertyType == typeof(short) || property.PropertyType == typeof(short?))
            {
                property.SetPropertyValueFromString(Convert.ToInt16, value, instance);
            }
            else if (property.PropertyType == typeof(decimal) || property.PropertyType == typeof(decimal?))
            {
                property.SetPropertyValueFromString(Convert.ToDecimal, value, instance);
            }
            else if (property.PropertyType == typeof(bool) || property.PropertyType == typeof(bool?))
            {
                property.SetPropertyValueFromString(Convert.ToBoolean, value, instance);
            }
            else if (property.PropertyType == typeof(byte) || property.PropertyType == typeof(byte?))
            {
                property.SetPropertyValueFromString(Convert.ToByte, value, instance);
            }
            else if (property.PropertyType == typeof(long) || property.PropertyType == typeof(long?))
            {
                property.SetPropertyValueFromString(Convert.ToInt64, value, instance);
            }
            else
            {
                throw new NotImplementedException("ReflectionExtensions.SetPropertyValueFromString(...) does not yet support this data type: " + property.PropertyType);
            }
        }

        private static void SetPropertyValueFromString<T>(this PropertyInfo property, Func<string, T> conversion, string value, object instance)
        {
            T convertedValue = value == null ? default(T) : conversion(value);

            property.SetValue(instance, convertedValue, null);
        }
    }
}