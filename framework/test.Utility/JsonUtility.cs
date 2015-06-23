using System;
using Newtonsoft.Json;

namespace test.Utility
{
    public static class JsonUtility
    {
        static JsonUtility()
        {
            //Json.Net配置
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore
                //DateFormatString = "yyyyMMdd"
            };
        }

        public static string Serialize<T>(T model)
        {
            return JsonConvert.SerializeObject(model);
        }

        public static string Serialize(object model)
        {
            if (model == null)
            {
                return string.Empty;
            }

            return JsonConvert.SerializeObject(model);
        }

        public static T Deserialize<T>(string src)
        {
            return JsonConvert.DeserializeObject<T>(src);
        }

        public static object Deserialize(string src, Type type)
        {
            return JsonConvert.DeserializeObject(src, type);
        }
    }
}