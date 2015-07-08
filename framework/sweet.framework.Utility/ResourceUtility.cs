using System.IO;
using System.Reflection;

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
        /// <returns></returns>
        public static string ReadTxt(string resourceName)
        {
            //读取嵌入式资源
            Assembly asm = Assembly.GetCallingAssembly();

            //当前程序集的名称，不是很准确，不是命名空间名称
            string fullName = asm.GetName().Name + @"." + resourceName;
            using (Stream stream = asm.GetManifestResourceStream(fullName))
            using (var reader = new StreamReader(stream))
            {
                var data = reader.ReadToEnd();
                return data;
            }
        }
    }
}