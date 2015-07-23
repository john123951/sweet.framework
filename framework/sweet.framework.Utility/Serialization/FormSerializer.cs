using System;
using System.Linq;
using System.Reflection;
using System.Text;

namespace sweet.framework.Utility.Serialization
{
    public static class FormSerializer
    {
        /// <summary>
        /// 序列化为Form表单形式（升序排列）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="valueSplit"></param>
        /// <param name="parmSplit"></param>
        /// <returns></returns>
        public static string Serialize<T>(T obj, string valueSplit = "=", string parmSplit = "&")
        {
            return Serialize(obj, valueSplit, parmSplit, true, true, true);
        }

        /// <summary>
        /// 序列化为Form表单形式
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="valueSplit"></param>
        /// <param name="parmSplit"></param>
        /// <param name="ignoreNull">忽略空值</param>
        /// <param name="enableSort">启用排序</param>
        /// <param name="asc">升序</param>
        /// <returns></returns>
        public static string Serialize<T>(T obj, string valueSplit, string parmSplit, bool ignoreNull, bool enableSort, bool asc)
        {
            if (obj == null) { return string.Empty; }
            if (valueSplit == null) { valueSplit = string.Empty; }
            if (parmSplit == null) { parmSplit = string.Empty; }

            var sbForm = new StringBuilder();

            IQueryable<PropertyInfo> propsLabmda = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance).AsQueryable();

            if (enableSort) { propsLabmda = asc ? propsLabmda.OrderBy(x => x.Name) : propsLabmda.OrderByDescending(x => x.Name); }

            var props = propsLabmda.ToList();

            foreach (var item in props)
            {
                object value = item.GetValue(obj, null);
                if (value == null) { value = string.Empty; }

                if (ignoreNull && value.ToString().Length <= 0)
                {
                    continue;
                }

                sbForm.Append(item.Name);
                sbForm.Append(valueSplit);
                sbForm.Append(value);
                sbForm.Append(parmSplit);
            }

            return sbForm.ToString(0, sbForm.Length - parmSplit.Length);
        }

        /// <summary>
        /// 反序列化Form数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="src"></param>
        /// <returns></returns>
        public static T Deserialize<T>(string src)
            where T : class, new()
        {
            var model = new T();

            if (string.IsNullOrEmpty(src))
            {
                return model;
            }

            var props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            string[] tmpResult = src.Split(new string[] { "&" }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var nameValue in tmpResult)
            {
                int index = nameValue.IndexOf("=", 0, StringComparison.Ordinal);

                var name = nameValue.Substring(0, index);
                var value = nameValue.Substring(index + 1);

                if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(value))
                {
                    var propInfo = props.FirstOrDefault(x => string.Compare(x.Name, name, System.StringComparison.OrdinalIgnoreCase) == 0);
                    if (propInfo != null)
                    {
                        propInfo.SetValue(model, value, null);
                    }
                }
            }

            return model;
        }
    }
}