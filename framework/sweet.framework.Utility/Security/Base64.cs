using System;
using System.Text;

namespace test.Utility.Security
{
    /// <summary>
    /// 实现Base64加密解密
    /// </summary>
    public static class Base64
    {
        /// <summary>
        /// Base64加密
        /// </summary>
        /// <param name="encoding">加密采用的编码方式</param>
        /// <param name="source">待加密的明文</param>
        /// <returns></returns>
        public static string EncodeBase64(Encoding encoding, string source)
        {
            byte[] bytes = encoding.GetBytes(source);
            return EncodeBase64(bytes);
        }

        /// <summary>
        /// Base64加密，采用utf8编码方式加密
        /// </summary>
        /// <param name="source">待加密的明文</param>
        /// <returns>加密后的字符串</returns>
        public static string EncodeBase64(string source)
        {
            return EncodeBase64(Encoding.UTF8, source);
        }

        /// <summary>
        /// Base64加密，采用utf8编码方式加密
        /// </summary>
        /// <param name="source">待加密的明文</param>
        /// <returns>加密后的字符串</returns>
        public static string EncodeBase64(byte[] source)
        {
            return Convert.ToBase64String(source);
        }

        /// <summary>
        /// Base64加密
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static byte[] EncodeBase64_byte(string source)
        {
            int modeX = source.Length % 4;
            if (modeX != 0)
            {
                for (int i = 0; i < 4 - modeX; i++)
                {
                    source = source + "=";
                }
            }

            byte[] bytes = Encoding.UTF8.GetBytes(source);

            return bytes;
        }

        /// <summary>
        /// Base64解密
        /// </summary>
        /// <param name="encoding">解密采用的编码方式，注意和加密时采用的方式一致</param>
        /// <param name="result">待解密的密文</param>
        /// <returns>解密后的字符串</returns>
        public static string DecodeBase64(Encoding encoding, string result)
        {
            string decode;
            byte[] bytes = Convert.FromBase64String(result);

            try
            {
                decode = encoding.GetString(bytes);
            }
            catch
            {
                decode = result;
            }
            return decode;
        }

        /// <summary>
        /// Base64解密，采用utf8编码方式解密
        /// </summary>
        /// <param name="result">待解密的密文</param>
        /// <returns>解密后的字符串</returns>
        public static string DecodeBase64(string result)
        {
            int modeX = result.Length % 4;
            if (modeX != 0)
            {
                for (int i = 0; i < 4 - modeX; i++)
                {
                    result = result + "=";
                }
            }
            return DecodeBase64(Encoding.UTF8, result);
        }

        /// <summary>
        /// Base64解密 ///
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public static byte[] DecodeBase64_byte(string result)
        {
            int modeX = result.Length % 4;
            if (modeX != 0)
            {
                for (int i = 0; i < 4 - modeX; i++)
                {
                    result = result + "=";
                }
            }
            byte[] bytes = Convert.FromBase64String(result);
            return bytes;
        }
    }
}