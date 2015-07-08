using System;
using System.Diagnostics;
using System.Reflection;

namespace sweet.framework.Utility
{
    /// <summary>
    /// 版本信息
    /// </summary>
    public static class VersionUtility
    {
        /// <summary>
        /// 获取当前程序集版本
        /// </summary>
        /// <returns></returns>
        public static Version GetAssemblyVersion()
        {
            return GetAssemblyVersion(Assembly.GetCallingAssembly());
        }

        /// <summary>
        /// 获取程序集版本
        /// </summary>
        /// <param name="asm"></param>
        /// <returns></returns>
        public static Version GetAssemblyVersion(Assembly asm)
        {
            return asm.GetName().Version;
        }

        /// <summary>
        /// 返回文件的版本信息
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetFileVersion(string fileName)
        {
            FileVersionInfo myFileVersion = FileVersionInfo.GetVersionInfo(fileName);
            return myFileVersion.FileVersion;
        }

        public static void GetProductVersion()
        {
            //return System.Windows.Forms.Application.ProductVersion;
        }
    }
}