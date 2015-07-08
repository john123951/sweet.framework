using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace sweet.framework.Utility.Serialization
{
    public static class XmlUtility
    {
        public static string Serialize<T>(T model)
        {
            var xmlSerializer = new XmlSerializer(typeof(T));

            using (var writer = new StringWriter())
            {
                xmlSerializer.Serialize(writer, model);
                return writer.ToString();
            }
        }

        public static T Deserialize<T>(string src)
        {
            var xmlSerializer = new XmlSerializer(typeof(T));

            using (var reader = new StringReader(src))
            {
                if (xmlSerializer.CanDeserialize(XmlReader.Create(reader)))
                {
                    return (T)xmlSerializer.Deserialize(reader);
                }
            }

            return default(T);
        }
    }
}