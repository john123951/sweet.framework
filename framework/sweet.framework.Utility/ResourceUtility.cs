using System.IO;
using System.Reflection;
using System.Text;

namespace sweet.framework.Utility
{
    /// <summary>
    /// 操作嵌入的资源
    /// </summary>
    public static class ResourceUtility
    {
        /// <summary>
        /// 以文本格式读取程序集中嵌入的资源
        /// </summary>
        /// <param name="resourceName"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string ReadString(string resourceName, Encoding encoding)
        {
            //读取嵌入式资源
            Assembly asm = Assembly.GetCallingAssembly();

            //当前程序集的名称(不是很准确，命名空间名称)
            string fullName = asm.GetName().Name + @"." + resourceName;

            using (Stream stream = asm.GetManifestResourceStream(fullName))
            {
                if (stream == null) { return string.Empty; }

                using (var reader = new StreamReader(stream, encoding))
                {
                    string data = reader.ReadToEnd();
                    return data;
                }
            }
        }

        /// <summary>
        /// 读取程序集中嵌入的资源
        /// </summary>
        /// <param name="resourceName"></param>
        /// <returns></returns>
        public static Stream ReadStream(string resourceName)
        {
            //读取嵌入式资源
            Assembly asm = Assembly.GetCallingAssembly();

            //当前程序集的名称(不是很准确，命名空间名称)
            string fullName = asm.GetName().Name + @"." + resourceName;

            Stream stream = asm.GetManifestResourceStream(fullName);

            return stream;
        }
    }
}