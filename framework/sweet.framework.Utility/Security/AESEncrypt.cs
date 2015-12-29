using Org.BouncyCastle.Utilities.Encoders;
using System;
using System.Security.Cryptography;
using System.Text;

namespace sweet.framework.Utility.Security
{
    public class AESEncrypt
    {
        private static readonly Encoding Encoding = Encoding.UTF8;

        /// <summary>
        /// 生成随即密钥，返回base64格式
        /// </summary>
        /// <param name="keySize">密钥长度</param>
        /// <returns></returns>
        public static string MakeKey(int keySize = 128)
        {
            var aesCryptoServiceProvider = new AesCryptoServiceProvider();
            aesCryptoServiceProvider.KeySize = keySize;

            aesCryptoServiceProvider.GenerateKey();
            byte[] key = aesCryptoServiceProvider.Key;

            return Convert.ToBase64String(key);
        }

        /// <summary>
        ///  AES 加密
        /// </summary>
        /// <param name="str"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string AesEncrypt(string str, string key)
        {
            if (string.IsNullOrEmpty(str)) return null;
            byte[] toEncryptArray = Encoding.UTF8.GetBytes(str);
            byte[] keyByte = Convert.FromBase64String(key);

            RijndaelManaged aes = new System.Security.Cryptography.RijndaelManaged
            {
                Key = keyByte,
                Mode = System.Security.Cryptography.CipherMode.ECB,
                Padding = System.Security.Cryptography.PaddingMode.PKCS7
            };

            ICryptoTransform cTransform = aes.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        /// <summary>
        ///  AES 解密
        /// </summary>
        /// <param name="str"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string AesDecrypt(string str, string key)
        {
            if (string.IsNullOrEmpty(str)) return null;
            byte[] toEncryptArray = Convert.FromBase64String(str);
            byte[] keyByte = Convert.FromBase64String(key);

            System.Security.Cryptography.RijndaelManaged aes = new System.Security.Cryptography.RijndaelManaged
            {
                Key = keyByte,
                Mode = System.Security.Cryptography.CipherMode.ECB,
                Padding = System.Security.Cryptography.PaddingMode.PKCS7
            };

            ICryptoTransform cTransform = aes.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return Encoding.GetString(resultArray);
        }

        public static string Encrypt_AES256(string plainText, string keyStr)
        {
            RijndaelManaged aes = new RijndaelManaged();
            aes.BlockSize = 128;
            aes.KeySize = 256;

            // It is equal in java
            // Cipher _Cipher = Cipher.getInstance("AES/CBC/PKCS5PADDING");
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            // Initialization vector.
            // It could be any value or generated using a random number generator.
            byte[] keyArr = Base64.Decode(keyStr);//密钥需是8的倍数个字节
            byte[] ivArr = { 1, 2, 3, 4, 5, 6, 6, 5, 4, 3, 2, 1, 7, 7, 7, 7 };

            aes.Key = keyArr;
            aes.IV = ivArr;

            ICryptoTransform encrypto = aes.CreateEncryptor();

            byte[] plainTextByte = Encoding.GetBytes(plainText);
            byte[] CipherText = encrypto.TransformFinalBlock(plainTextByte, 0, plainTextByte.Length);
            return Convert.ToBase64String(CipherText);
        }

        public static string Decrypt_AES256(string cipherText, string keyStr)
        {
            RijndaelManaged aes = new RijndaelManaged();
            aes.BlockSize = 128;
            aes.KeySize = 256;

            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            // Initialization vector.
            // It could be any value or generated using a random number generator.
            byte[] keyArr = Base64.Decode(keyStr);
            byte[] ivArr = { 1, 2, 3, 4, 5, 6, 6, 5, 4, 3, 2, 1, 7, 7, 7, 7 };

            aes.Key = keyArr;//密钥需是8的倍数个字节
            aes.IV = ivArr;

            ICryptoTransform decrypto = aes.CreateDecryptor();

            byte[] encryptedBytes = Convert.FromBase64CharArray(cipherText.ToCharArray(), 0, cipherText.Length);
            byte[] decryptedData = decrypto.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);
            return Encoding.GetString(decryptedData);
        }
    }
}