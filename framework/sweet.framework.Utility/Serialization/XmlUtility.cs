using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace sweet.framework.Utility.Serialization
{
    public static class XmlUtility
    {
        public static string Serialize(object obj)
        {
            if (obj == null) { return string.Empty; }

            XmlSerializer serializer = new XmlSerializer(obj.GetType());

            using (MemoryStream stream = new MemoryStream())
            {
                serializer.Serialize(stream, obj);
                stream.Position = 0;

                using (StreamReader reader = new StreamReader(stream))
                {
                    string str = reader.ReadToEnd();
                    return str;
                }
            }
        }

        private static string Serialize<T>(T model)
        {
            var xmlSerializer = new XmlSerializer(typeof(T));

            //XmlWriterSettings settings = new XmlWriterSettings();
            //settings.OmitXmlDeclaration = true;//这一句表示忽略xml声明
            //settings.Indent = true;
            //settings.Encoding = encoding;
            //XmlWriter tw = XmlWriter.Create(ms, settings);

            using (var writer = new StringWriter())
            {
                xmlSerializer.Serialize(writer, model);
                return writer.ToString();
            }
        }

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