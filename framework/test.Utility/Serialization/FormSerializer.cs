using System;
using System.Linq;
using System.Reflection;
using System.Text;

namespace test.Utility.Serialization
{
    public static class FormSerializer
    {
        public static string Serialize<T>(T obj, string valueSplit = "=", string parmSplit = "&")
        {
            if (obj == null) { return string.Empty; }
            if (valueSplit == null) { valueSplit = string.Empty; }
            if (parmSplit == null) { parmSplit = string.Empty; }

            var sbForm = new StringBuilder();

            var props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                 .OrderBy(x => x.Name)
                                 .ToList();

            foreach (var item in props)
            {
                object value = item.GetValue(obj, null);

                if (value != null && value.ToString().Length > 0)
                {
                    sbForm.Append(item.Name);
                    sbForm.Append(valueSplit);
                    sbForm.Append(value);
                    sbForm.Append(parmSplit);
                }
            }

            return sbForm.ToString(0, sbForm.Length - parmSplit.Length);
        }

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
                    var propInfo = props.FirstOrDefault(x => string.Compare(x.Name, name, true) == 0);
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