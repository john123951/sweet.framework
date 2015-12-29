using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace sweet.framework.Utility.Serialization
{
    public static class XmlUtility
    {
        private static Encoding Encoding = Encoding.UTF8;

        /// <summary>
        /// 序列化Xml
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string Serialize(object obj)
        {
            if (obj == null) { return string.Empty; }

            XmlSerializer serializer = new XmlSerializer(obj.GetType());

            using (MemoryStream stream = new MemoryStream())
            {
                //忽略命名空间
                var xmlns = new XmlSerializerNamespaces();
                xmlns.Add(string.Empty, string.Empty);

                serializer.Serialize(stream, obj, xmlns);
                stream.Position = 0;

                using (StreamReader reader = new StreamReader(stream))
                {
                    string str = reader.ReadToEnd();
                    return str;
                }
            }
        }

        /// <summary>
        /// 序列化Xml (不生成Xml声明)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string SerializeNoXmlDeclaration<T>(T model, Encoding encoding = null)
        {
            if (encoding == null) { encoding = Encoding; }

            var xmlSerializer = new XmlSerializer(typeof(T));

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.OmitXmlDeclaration = true;//这一句表示忽略xml声明
            settings.Indent = true;
            settings.Encoding = encoding;
            settings.CloseOutput = false;

            //忽略命名空间
            var xmlns = new XmlSerializerNamespaces();
            xmlns.Add(string.Empty, string.Empty);

            using (MemoryStream stream = new MemoryStream())
            //using (XmlWriter writer = XmlWriter.Create(stream, settings))
            {
                XmlWriter writer = XmlWriter.Create(stream, settings);
                xmlSerializer.Serialize(writer, model, xmlns);

                //return encoding.GetString(strm.ToArray(), 3, (int)strm.Length - 3);

                using (var streamReader = new StreamReader(stream, encoding))
                {
                    stream.Seek(0, System.IO.SeekOrigin.Begin);
                    string result = streamReader.ReadToEnd();

                    return result;
                }
            }
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="str"></param>
        /// <returns></returns>
        public static T Deserialize<T>(string str)
        {
            try
            {
                XmlDocument xdoc = new XmlDocument();
                xdoc.LoadXml(str);

                using (XmlNodeReader reader = new XmlNodeReader(xdoc.DocumentElement))
                {
                    XmlSerializer ser = new XmlSerializer(typeof(T));
                    object obj = ser.Deserialize(reader);

                    return (T)obj;
                }
            }
            catch
            {
                return default(T);
            }
        }
    }
}