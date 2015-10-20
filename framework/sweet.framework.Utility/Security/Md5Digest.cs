using System;
using System.Text;

namespace sweet.framework.Utility.Security
{
    /// <summary>
    /// MD5 无逆向编码
    /// 获取唯一特征串，可用于密码加密
    /// (无法还原)
    /// </summary>
    public class Md5Digest
    {
        /// <summary>
        /// MD5 16位加密
        /// </summary>
        /// <param name="inputString">输入文本</param>
        /// <returns></returns>
        public static string EncryptString_16(string inputString)
        {
            return EncryptString_16(inputString, Encoding.UTF8);
        }

        /// <summary>
        /// MD5 16位加密
        /// </summary>
        /// <param name="inputString">输入文本</param>
        /// <param name="encoding">字符编码</param>
        /// <returns></returns>
        public static string EncryptString_16(string inputString, System.Text.Encoding encoding)
        {
            System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();//实例化一个md5对像
            return BitConverter.ToString(md5.ComputeHash(encoding.GetBytes(inputString)), 4, 8).Replace("-", "").ToLower();
        }

        /// <summary>
        /// MD5　32位加密
        /// </summary>
        /// <param name="inputString"></param>
        /// <returns></returns>
        public static string EncryptString_32(string inputString)
        {
            return EncryptString_32(inputString, System.Text.Encoding.UTF8);
        }

        /// <summary>
        /// MD5　32位加密
        /// </summary>
        /// <param name="inputString">输入文本</param>
        /// <param name="encoding">字符编码</param>
        /// <returns></returns>
        public static string EncryptString_32(string inputString, System.Text.Encoding encoding)
        {
            string pwd = string.Empty;
            System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();//实例化一个md5对像

            // 加密后是一个字节类型的数组，这里要注意编码UTF8/Unicode等的选择　
            byte[] hash = md5.ComputeHash(encoding.GetBytes(inputString));

            // 通过使用循环，将字节类型的数组转换为字符串，此字符串是常规字符格式化所得
            for (int i = 0; i < hash.Length; i++)
            {
                // 将得到的字符串使用十六进制类型格式。格式后的字符是小写的字母，如果使用大写（X）则格式后的字符是大写字符
                pwd = pwd + hash[i].ToString("x2");
            }
            return pwd;
        }

        /// <summary>
        /// MD5　32位加密
        /// </summary>
        /// <param name="inputString">输入文本</param>
        /// <returns></returns>
        public static string EncryptString(string inputString)
        {
            return EncryptString(inputString, System.Text.Encoding.UTF8);
        }

        /// <summary>
        /// MD5　32位加密
        /// </summary>
        /// <param name="inputString">输入文本</param>
        /// <param name="encoding">字符编码</param>
        /// <returns></returns>
        public static string EncryptString(string inputString, System.Text.Encoding encoding)
        {
            System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
            return BitConverter.ToString(md5.ComputeHash(encoding.GetBytes(inputString))).Replace("-", "").ToLower();
        }
    }
}