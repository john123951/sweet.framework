using System;
using System.Text;

namespace sweet.framework.Utility.Security
{
    /// <summary>
    /// 实现Base64编码解码
    /// </summary>
    public static class Base64Utility
    {
        /// <summary>
        /// Base64编码，采用utf8编码方式编码
        /// </summary>
        /// <param name="source">待编码的明文</param>
        /// <returns>编码后的字符串</returns>
        public static string EncodeBase64(string source)
        {
            return EncodeBase64(source, Encoding.UTF8);
        }

        /// <summary>
        /// Base64编码
        /// </summary>
        /// <param name="source">待编码的明文</param>
        /// <param name="encoding">编码采用的编码方式</param>
        /// <returns></returns>
        public static string EncodeBase64(string source, Encoding encoding)
        {
            byte[] bytes = encoding.GetBytes(source);
            return EncodeBase64(bytes);
        }

        /// <summary>
        /// Base64编码，采用utf8编码方式编码
        /// </summary>
        /// <param name="bytes">待编码的明文</param>
        /// <returns>编码后的字符串</returns>
        public static string EncodeBase64(byte[] bytes)
        {
            return Convert.ToBase64String(bytes);
        }

        /// <summary>
        /// Base64解码，采用utf8编码方式解码
        /// </summary>
        /// <param name="base64Data">待解码的密文</param>
        /// <returns>解码后的字符串</returns>
        public static string DecodeBase64(string base64Data)
        {
            return DecodeBase64(base64Data, Encoding.UTF8);
        }

        /// <summary>
        /// Base64解码
        /// </summary>
        /// <param name="base64Data">待解码的密文</param>
        /// <param name="encoding">解码采用的编码方式，注意和编码时采用的方式一致</param>
        /// <returns>解码后的字符串</returns>
        public static string DecodeBase64(string base64Data, Encoding encoding)
        {
            byte[] bytes = DecodeBase64_byte(base64Data);
            string rawData = encoding.GetString(bytes);

            return rawData;
        }

        /// <summary>
        /// Base64解码 ///
        /// </summary>
        /// <param name="base64Data"></param>
        /// <returns></returns>
        public static byte[] DecodeBase64_byte(string base64Data)
        {
            int modeX = base64Data.Length % 4;
            if (modeX != 0)
            {
                for (int i = 0; i < 4 - modeX; i++)
                {
                    base64Data = base64Data + "=";
                }
            }
            byte[] bytes = Convert.FromBase64String(base64Data);
            return bytes;
        }
    }
}