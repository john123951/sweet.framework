using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities.Encoders;
using Org.BouncyCastle.X509;

namespace test.Utility.Security
{
    public class RSA
    {
        #region 设置

        /// <summary>
        /// RSA最大加密明文大小
        /// </summary>
        private const int MAX_ENCRYPT_BLOCK = 117;

        /// <summary>
        /// RSA最大解密密文大小
        /// </summary>
        private const int MAX_DECRYPT_BLOCK = 256;

        /// <summary>
        /// 编码格式
        /// </summary>
        private static readonly Encoding Encoding = Encoding.UTF8;

        #endregion 设置

        #region 生成密钥对

        /// <summary>
        /// 生成密钥对
        /// </summary>
        /// <param name="directoryPath">生成目录</param>
        public static void GenneratRsaKey(string directoryPath)
        {
            if (string.IsNullOrEmpty(directoryPath))
            {
                throw new ArgumentNullException("directoryPath");
            }

            RsaKeyPairGenerator rsaKeyPairGenerator = new RsaKeyPairGenerator();
            RsaKeyGenerationParameters rsaKeyGenerationParameters = new RsaKeyGenerationParameters(Org.BouncyCastle.Math.BigInteger.ValueOf(3), new SecureRandom(), 2048, 25);
            rsaKeyPairGenerator.Init(rsaKeyGenerationParameters);//初始化参数
            AsymmetricCipherKeyPair keyPair = rsaKeyPairGenerator.GenerateKeyPair();
            AsymmetricKeyParameter publicKey = keyPair.Public;//公钥
            AsymmetricKeyParameter privateKey = keyPair.Private;//私钥

            SubjectPublicKeyInfo subjectPublicKeyInfo = SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo(publicKey);
            PrivateKeyInfo privateKeyInfo = PrivateKeyInfoFactory.CreatePrivateKeyInfo(privateKey);

            Asn1Object asn1ObjectPublic = subjectPublicKeyInfo.ToAsn1Object();
            byte[] publicInfoByte = asn1ObjectPublic.GetEncoded();

            Asn1Object asn1ObjectPrivate = privateKeyInfo.ToAsn1Object();
            byte[] privateInfoByte = asn1ObjectPrivate.GetEncoded();

            //生成操作
            Directory.CreateDirectory(directoryPath);

            #region 生成xml

            var rsa = new RSACryptoServiceProvider();
            var key = (RsaPrivateCrtKeyParameters)keyPair.Private;
            var p = new RSAParameters
            {
                Modulus = key.Modulus.ToByteArrayUnsigned(),
                Exponent = key.PublicExponent.ToByteArrayUnsigned(),
                D = key.Exponent.ToByteArrayUnsigned(),
                P = key.P.ToByteArrayUnsigned(),
                Q = key.Q.ToByteArrayUnsigned(),
                DP = key.DP.ToByteArrayUnsigned(),
                DQ = key.DQ.ToByteArrayUnsigned(),
                InverseQ = key.QInv.ToByteArrayUnsigned(),
            };
            rsa.ImportParameters(p);

            string filePath_xmlPriPub = Path.Combine(directoryPath, "priAndPub.xml");
            string filePath_xmlPub = Path.Combine(directoryPath, "pub.xml");
            File.WriteAllText(filePath_xmlPriPub, rsa.ToXmlString(true));
            File.WriteAllText(filePath_xmlPub, rsa.ToXmlString(false));

            #endregion 生成xml

            //这里可以将密钥对保存到本地
            string filePath_publickey = Path.Combine(directoryPath, "netPublic.key");
            string filePath_privatekey = Path.Combine(directoryPath, "netPrivate.key");
            File.WriteAllText(filePath_publickey, Convert.ToBase64String(publicInfoByte));
            File.WriteAllText(filePath_privatekey, Convert.ToBase64String(privateInfoByte));

            Console.WriteLine("PublicKey:\n" + Convert.ToBase64String(publicInfoByte));
            Console.WriteLine("PrivateKey:\n" + Convert.ToBase64String(privateInfoByte));
            Console.WriteLine("OK");
        }

        #endregion 生成密钥对

        #region 加解密

        /// <summary>
        /// 数据加密处理
        /// </summary>
        /// <param name="orgData">源数据</param>
        /// <param name="publicKey">公钥(BASE64编码)</param>
        /// <returns></returns>
        public static string Encrypt(string orgData, string publicKey)
        {
            byte[] source = Encoding.GetBytes(orgData);
            IAsymmetricBlockCipher engine = new RsaEngine();
            AsymmetricKeyParameter publicK = PublicKeyFactory.CreateKey(Base64.Decode(publicKey));

            // 此处填充方式选择部填充 NoPadding，当然模式和填充方式选择其他的，在Java端可以正确加密解密，
            // 但是解密后的密文提交给C#端，解密的得到的数据将产生乱码
            engine.Init(true, publicK);
            int length = source.Length;
            int offset = 0;
            byte[] cache;

            using (MemoryStream outStream = new MemoryStream())
            {
                int i = 0;
                while (length - offset > 0)
                {
                    if (length - offset > MAX_ENCRYPT_BLOCK)
                    {
                        cache = engine.ProcessBlock(source, offset, MAX_ENCRYPT_BLOCK);
                    }
                    else
                    {
                        cache = engine.ProcessBlock(source, offset, length - offset);
                    }
                    outStream.Write(cache, 0, cache.Length);
                    i++;
                    offset = i * MAX_ENCRYPT_BLOCK;
                }
                return Encoding.GetString(Base64.Encode(outStream.ToArray()));
            }
        }

        /// <summary>
        /// 数据解密处理
        /// </summary>
        /// <param name="encryptString">已加密数据</param>
        /// <param name="privateKey">私钥(BASE64编码)</param>
        /// <returns></returns>
        public static string Decrypt(string encryptString, string privateKey)
        {
            byte[] encryptData = Base64.Decode(encryptString.Trim());

            IAsymmetricBlockCipher engine = new RsaEngine();
            AsymmetricKeyParameter priKey = PrivateKeyFactory.CreateKey(Base64.Decode(privateKey));

            engine.Init(false, priKey);

            //分段解密 解决加密密文过长问题
            int length = encryptData.Length;
            int offset = 0;
            int i = 0;
            using (MemoryStream outStream = new MemoryStream())
            {
                while (length - offset > 0)
                {
                    byte[] cache;
                    if (length - offset > MAX_DECRYPT_BLOCK)
                    {
                        cache = engine.ProcessBlock(encryptData, offset, MAX_DECRYPT_BLOCK);
                    }
                    else
                    {
                        cache = engine.ProcessBlock(encryptData, offset, length - offset);
                    }
                    outStream.Write(cache, 0, cache.Length);
                    i++;
                    offset = i * MAX_DECRYPT_BLOCK;
                }

                using (var sReader = new StreamReader(outStream, Encoding))
                {
                    outStream.Position = 0;
                    string result = sReader.ReadToEnd();
                    return result;
                }
            }
        }

        /// <summary>
        /// RSA加密
        /// </summary>
        /// <param name="publickey"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string XMLEncrypt(string publickey, string content)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(publickey);

            byte[] cipherbytes = rsa.Encrypt(Encoding.GetBytes(content), false);

            return Encoding.GetString(Base64.Encode(cipherbytes));
        }

        /// <summary>
        /// RSA解密
        /// </summary>
        /// <param name="privateKey"></param>
        /// <param name="encryptString"></param>
        /// <returns></returns>
        public static string XMLDecrypt(string privateKey, string encryptString)
        {
            byte[] encryptData = Base64.Decode(encryptString.Trim());

            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(privateKey);
            byte[] dypherTextBArray = rsa.Decrypt(encryptData, false);
            string result = Encoding.GetString(dypherTextBArray);
            return result;
        }

        #endregion 加解密

        #region 加签、验签

        /// <summary>
        /// 加签 sign the data
        /// </summary>
        /// <param name="dataToBeSigned">要加签的数据</param>
        /// <param name="privateKey">私钥</param>
        /// <returns></returns>
        public static string SignData(string dataToBeSigned, string privateKey)
        {
            byte[] data = Base64.Decode(dataToBeSigned);
            var rsa = new RSACryptoServiceProvider();
            RsaPrivateCrtKeyParameters priKey = (RsaPrivateCrtKeyParameters)PrivateKeyFactory.CreateKey(Base64.Decode(privateKey));
            var p = new RSAParameters
            {
                Modulus = priKey.Modulus.ToByteArrayUnsigned(),
                Exponent = priKey.PublicExponent.ToByteArrayUnsigned(),
                D = priKey.Exponent.ToByteArrayUnsigned(),
                P = priKey.P.ToByteArrayUnsigned(),
                Q = priKey.Q.ToByteArrayUnsigned(),
                DP = priKey.DP.ToByteArrayUnsigned(),
                DQ = priKey.DQ.ToByteArrayUnsigned(),
                InverseQ = priKey.QInv.ToByteArrayUnsigned(),
            };
            rsa.ImportParameters(p);

            byte[] endata = rsa.SignData(data, typeof(SHA1));

            return Encoding.GetString(Base64.Encode(endata));
        }

        /// <summary>
        /// 验签 Verifies the signature for a given data.
        /// </summary>
        /// <param name="signature"> 要验证的签名（Base64）</param>
        /// <param name="signedData">签名前的原始数据（Base64）</param>
        /// <param name="publicKey">公钥(BASE64编码)</param>
        /// <returns></returns>
        public static bool VerifySignature(string signature, string signedData, string publicKey)
        {
            RsaKeyParameters publicK = (RsaKeyParameters)PublicKeyFactory.CreateKey(Base64.Decode(publicKey));

            var rsa = new RSACryptoServiceProvider();
            var rsaKeyInfo = new RSAParameters
            {
                Modulus = publicK.Modulus.ToByteArrayUnsigned(),
                Exponent = publicK.Exponent.ToByteArrayUnsigned(),
            };
            rsa.ImportParameters(rsaKeyInfo);

            byte[] buffer = Base64.Decode(signedData);
            byte[] sign = Base64.Decode(signature);
            // 参数:
            //   buffer:
            //     已签名的数据。
            //
            //   halg:
            //     用于创建数据的哈希值的哈希算法名称。
            //
            //   signature:
            //     要验证的签名数据。
            if (rsa.VerifyData(buffer, typeof(SHA1), sign))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 加签 sign the data
        /// </summary>
        /// <param name="dataToBeSigned">要加签的数据</param>
        /// <param name="xmlContent">私钥</param>
        /// <returns></returns>
        public static string SignDataByXml(string dataToBeSigned, string xmlContent)
        {
            byte[] data = Base64.Decode(dataToBeSigned);
            var rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(xmlContent);
            byte[] endata = rsa.SignData(data, typeof(SHA1));

            return Encoding.GetString(Base64.Encode(endata));
        }

        /// <summary>
        /// 验签 Verifies the signature for a given data.
        /// </summary>
        /// <param name="xmlPublickeyStream">公钥（XML格式）</param>
        /// <param name="signature">要验证的签名（Base64）</param>
        /// <param name="signedData">签名前的原始数据（Base64）</param>
        /// <returns>True if signature is valid else False</returns>
        public static bool VerifySignatureByXml(string signature, string signedData, Stream xmlPublickeyStream)
        {
            var doc = XDocument.Load(xmlPublickeyStream);

            //公钥参数
            if (doc.Root == null)
            {
                return false;
            }

            var xElement = doc.Root.Element("Modulus");
            var element = doc.Root.Element("Exponent");

            if (xElement == null || element == null)
            {
                return false;
            }

            //公钥参数
            string PUB_KEY_MODULES = xElement.Value;
            string PUB_KEY_EXP = element.Value;

            byte[] sign = Convert.FromBase64String(signature);
            byte[] buffer = Convert.FromBase64String(signedData);

            //验签
            using (var rsa = new RSACryptoServiceProvider())
            {
                var rsaKeyInfo = new RSAParameters();
                rsaKeyInfo.Modulus = Convert.FromBase64String(PUB_KEY_MODULES);
                rsaKeyInfo.Exponent = Convert.FromBase64String(PUB_KEY_EXP);
                rsa.ImportParameters(rsaKeyInfo);

                // 参数:
                //   buffer:
                //     已签名的数据。
                //
                //   halg:
                //     用于创建数据的哈希值的哈希算法名称。
                //
                //   signature:
                //     要验证的签名数据。
                if (rsa.VerifyData(buffer, typeof(SHA1), sign))
                {
                    return true;
                }
            }

            return false;
        }

        #endregion 加签、验签

        /// <summary>
        /// 解决PHP Base64编码后回车问题
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string Decode64(string text)
        {
            StringBuilder sbResult = new StringBuilder();

            text = string.IsNullOrEmpty(text) ? "" : text;

            int len = 64;
            int m = text.Length / len;
            if (m * len != text.Length)
            {
                m = m + 1;
            }

            for (int i = 0; i < m; i++)
            {
                string temp;

                if (i < m - 1)
                {
                    temp = text.Substring(i * len, len); //(i + 1) * len
                    sbResult.Append(temp + "\r\n");
                }
                else
                {
                    temp = text.Substring(i * len);
                    sbResult.Append(temp);
                }
            }

            return sbResult.ToString();
        }
    }
}