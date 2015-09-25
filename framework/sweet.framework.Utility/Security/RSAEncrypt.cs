#define PKCS1Padding

using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;

namespace sweet.framework.Utility.Security
{
    public class RSAEncrypt
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
        public static void GenerateRsaKey(string directoryPath)
        {
            if (string.IsNullOrEmpty(directoryPath))
            {
                throw new ArgumentNullException("directoryPath");
            }

            RsaKeyPairGenerator rsaKeyPairGenerator = new RsaKeyPairGenerator();
            RsaKeyGenerationParameters rsaKeyGenerationParameters =
                new RsaKeyGenerationParameters(Org.BouncyCastle.Math.BigInteger.ValueOf(3), new SecureRandom(), 2048, 25);
            rsaKeyPairGenerator.Init(rsaKeyGenerationParameters); //初始化参数
            AsymmetricCipherKeyPair keyPair = rsaKeyPairGenerator.GenerateKeyPair();
            AsymmetricKeyParameter publicKey = keyPair.Public; //公钥
            AsymmetricKeyParameter privateKey = keyPair.Private; //私钥

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

        #region 加密解密

        /// <summary>
        /// 数据加密处理
        /// </summary>
        /// <param name="orgData">源数据</param>
        /// <param name="publicKey">公钥(BASE64编码)</param>
        /// <returns></returns>
        public static string Encrypt(string orgData, string publicKey)
        {
            byte[] source = Encoding.GetBytes(orgData);

            #region NoPadding

            //IAsymmetricBlockCipher engine = new RsaEngine();
            //AsymmetricKeyParameter publicK = PublicKeyFactory.CreateKey(Org.BouncyCastle.Utilities.Encoders.Base64.Decode(publicKey));
            //engine.Init(true, publicK);

            #endregion NoPadding

            #region RSA/ECB/PKCS1Padding

            //RsaKeyParameters publicK = (Org.BouncyCastle.Crypto.Parameters.RsaKeyParameters)PublicKeyFactory.CreateKey(Org.BouncyCastle.Utilities.Encoders.Base64.Decode(publicKey));

            //IAsymmetricBlockCipher engine = new Pkcs1Encoding(new RsaEngine()); //使用"RSA/ECB/PKCS1Padding"方式?
            //RsaKeyParameters pubParameters = new RsaKeyParameters(false, publicK.Modulus, publicK.Exponent);

            //engine.Init(true, pubParameters);

            #endregion RSA/ECB/PKCS1Padding

            #region RSA/ECB/PKCS1Padding

            //RsaKeyParameters publicK = (Org.BouncyCastle.Crypto.Parameters.RsaKeyParameters)PublicKeyFactory.CreateKey(Org.BouncyCastle.Utilities.Encoders.Base64.Decode(publicKey));
            //RsaKeyParameters pubParameters = new RsaKeyParameters(false, publicK.Modulus, publicK.Exponent);

            //IBufferedCipher engine = CipherUtilities.GetCipher("RSA/ECB/PKCS1Padding");
            //engine.Init(true, pubParameters);

            #endregion RSA/ECB/PKCS1Padding

#if PKCS1Padding
            RsaKeyParameters publicK =
                (Org.BouncyCastle.Crypto.Parameters.RsaKeyParameters)
                    PublicKeyFactory.CreateKey(Org.BouncyCastle.Utilities.Encoders.Base64.Decode(publicKey));
            RsaKeyParameters pubParameters = new RsaKeyParameters(false, publicK.Modulus, publicK.Exponent);

            IBufferedCipher engine = CipherUtilities.GetCipher("RSA/ECB/PKCS1Padding");
            engine.Init(true, pubParameters);
#else
            IAsymmetricBlockCipher engine = new Org.BouncyCastle.Crypto.Engines.RsaEngine();
            AsymmetricKeyParameter publicK = PublicKeyFactory.CreateKey(Org.BouncyCastle.Utilities.Encoders.Base64.Decode(publicKey));
            engine.Init(true, publicK);
#endif

            int length = source.Length;
            int offset = 0;

            using (MemoryStream outStream = new MemoryStream())
            {
                int i = 0;
                while (length - offset > 0)
                {
                    byte[] buff;
                    if (length - offset > MAX_ENCRYPT_BLOCK)
                    {
#if PKCS1Padding
                        buff = engine.DoFinal(source, offset, MAX_ENCRYPT_BLOCK);
                    }
                    else
                    {
                        buff = engine.DoFinal(source, offset, length - offset);
#else
                        buff = engine.ProcessBlock(source, offset, MAX_ENCRYPT_BLOCK);
                    }
                    else
                    {
                        buff = engine.ProcessBlock(source, offset, length - offset);
#endif
                    }
                    outStream.Write(buff, 0, buff.Length);
                    i++;
                    offset = i * MAX_ENCRYPT_BLOCK;
                }
                return Encoding.GetString(Org.BouncyCastle.Utilities.Encoders.Base64.Encode(outStream.ToArray()));
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
            byte[] encryptData = Org.BouncyCastle.Utilities.Encoders.Base64.Decode(encryptString.Trim());

            #region NoPadding

            //IAsymmetricBlockCipher engine = new RsaEngine();
            //AsymmetricKeyParameter priKey = PrivateKeyFactory.CreateKey(Org.BouncyCastle.Utilities.Encoders.Base64.Decode(privateKey));

            //engine.Init(false, priKey);

            #endregion NoPadding

            #region RSA/ECB/PKCS1Padding

            //RsaPrivateCrtKeyParameters priKey = (RsaPrivateCrtKeyParameters)PrivateKeyFactory.CreateKey(Org.BouncyCastle.Utilities.Encoders.Base64.Decode(privateKey));

            //IAsymmetricBlockCipher engine = new RsaEngine();
            //RsaKeyParameters priParameters = new RsaPrivateCrtKeyParameters(priKey.Modulus, priKey.PublicExponent, priKey.Exponent, priKey.P, priKey.Q, priKey.DP, priKey.DQ, priKey.QInv);

            //engine.Init(false, priParameters);

            #endregion RSA/ECB/PKCS1Padding

            #region RSA/ECB/PKCS1Padding

            //RsaPrivateCrtKeyParameters priKey = (RsaPrivateCrtKeyParameters)PrivateKeyFactory.CreateKey(Org.BouncyCastle.Utilities.Encoders.Base64.Decode(privateKey));
            //RsaKeyParameters priParameters = new RsaPrivateCrtKeyParameters(priKey.Modulus, priKey.PublicExponent, priKey.Exponent, priKey.P, priKey.Q, priKey.DP, priKey.DQ, priKey.QInv);

            //IBufferedCipher engine = CipherUtilities.GetCipher("RSA/ECB/PKCS1Padding");
            //engine.Init(false, priParameters);

            #endregion RSA/ECB/PKCS1Padding

#if PKCS1Padding

            RsaPrivateCrtKeyParameters priKey =
                (RsaPrivateCrtKeyParameters)
                    PrivateKeyFactory.CreateKey(Org.BouncyCastle.Utilities.Encoders.Base64.Decode(privateKey));
            RsaKeyParameters priParameters = new RsaPrivateCrtKeyParameters(priKey.Modulus, priKey.PublicExponent,
                priKey.Exponent, priKey.P, priKey.Q, priKey.DP, priKey.DQ, priKey.QInv);

            IBufferedCipher engine = CipherUtilities.GetCipher("RSA/ECB/PKCS1Padding");
            engine.Init(false, priParameters);
#else
            IAsymmetricBlockCipher engine = new Org.BouncyCastle.Crypto.Engines.RsaEngine();
            AsymmetricKeyParameter priKey = PrivateKeyFactory.CreateKey(Org.BouncyCastle.Utilities.Encoders.Base64.Decode(privateKey));

            engine.Init(false, priKey);
#endif
            //分段解密 解决加密密文过长问题
            int length = encryptData.Length;
            int offset = 0;
            int i = 0;
            using (MemoryStream outStream = new MemoryStream())
            {
                while (length - offset > 0)
                {
                    byte[] buff;
                    if (length - offset > MAX_DECRYPT_BLOCK)
                    {
#if PKCS1Padding
                        buff = engine.DoFinal(encryptData, offset, MAX_DECRYPT_BLOCK);
                    }
                    else
                    {
                        buff = engine.DoFinal(encryptData, offset, length - offset);
                    }
#else
                        buff = engine.ProcessBlock(encryptData, offset, MAX_DECRYPT_BLOCK);
                    }
                    else
                    {
                        buff = engine.ProcessBlock(encryptData, offset, length - offset);
                    }
#endif
                    outStream.Write(buff, 0, buff.Length);
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

        #endregion 加密解密

        #region 加签、验签

        /// <summary>
        /// 加签 sign the data
        /// </summary>
        /// <param name="dataToBeSigned">要加签的数据</param>
        /// <param name="xmlPrivateKey">XML私钥</param>
        /// <returns></returns>
        public static string SignDataMicrosoft(string dataToBeSigned, string xmlPrivateKey)
        {
            byte[] data = Convert.FromBase64String(dataToBeSigned);

            var rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(xmlPrivateKey);

            byte[] endata = rsa.SignData(data, typeof(SHA1));

            return Convert.ToBase64String(endata);
        }

        /// <summary>
        /// 加签 sign the data
        /// </summary>
        /// <param name="dataToBeSigned">要加签的数据</param>
        /// <param name="privateKey">私钥</param>
        /// <returns></returns>
        public static string SignData(string dataToBeSigned, string privateKey)
        {
            byte[] data = Org.BouncyCastle.Utilities.Encoders.Base64.Decode(dataToBeSigned);

            RsaPrivateCrtKeyParameters priKey =
                (RsaPrivateCrtKeyParameters)
                    PrivateKeyFactory.CreateKey(Org.BouncyCastle.Utilities.Encoders.Base64.Decode(privateKey));

            ISigner verifier = SignerUtilities.GetSigner("SHA-1withRSA");
            verifier.Init(true, priKey);

            verifier.BlockUpdate(data, 0, data.Length);
            var sigBytes = verifier.GenerateSignature();

            return Encoding.GetString(Org.BouncyCastle.Utilities.Encoders.Base64.Encode(sigBytes));
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

        /// <summary>
        /// 加签 sign the data
        /// </summary>
        /// <param name="signature"> 要验证的签名数据（Base64）</param>
        /// <param name="signedData">签名前的原始数据（Base64）</param>
        /// <param name="xmlPublicKey">XML公钥</param>
        /// <returns></returns>
        public static bool VerifySignatureMicrosoft(string signature, string signedData, string xmlPublicKey)
        {
            var rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(xmlPublicKey);

            byte[] buffer = Org.BouncyCastle.Utilities.Encoders.Base64.Decode(signedData);
            byte[] sign = Org.BouncyCastle.Utilities.Encoders.Base64.Decode(signature);

            // 参数:
            //   buffer:
            //     已签名的数据。
            //
            //   halg:
            //     用于创建数据的哈希值的哈希算法名称。
            //
            //   signature:
            //     要验证的签名数据。
            return rsa.VerifyData(buffer, typeof(SHA1), sign);
        }

        /// <summary>
        /// 验签
        /// </summary>
        /// <param name="signature"> 要验证的签名数据（Base64）</param>
        /// <param name="signedData">签名前的原始数据（Base64）</param>
        /// <param name="publicKey">公钥</param>
        /// <returns></returns>
        public static bool VerifySignature(string signature, string signedData, string publicKey)
        {
            byte[] sign = Org.BouncyCastle.Utilities.Encoders.Base64.Decode(signature);
            byte[] buffer = Org.BouncyCastle.Utilities.Encoders.Base64.Decode(signedData);

            AsymmetricKeyParameter publicK =
                PublicKeyFactory.CreateKey(Org.BouncyCastle.Utilities.Encoders.Base64.Decode(publicKey));

            ISigner verifier = SignerUtilities.GetSigner("SHA-1withRSA");
            verifier.Init(false, publicK);

            verifier.BlockUpdate(buffer, 0, buffer.Length);

            return verifier.VerifySignature(sign);
        }

        #endregion 加签、验签

        #region XML与Base64转换

        public static string PrivateKeyXmlToBase64(string xmlPrivateKey)
        {
            var rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(xmlPrivateKey);

            var priKey = rsa.ExportParameters(true);
            RsaPrivateCrtKeyParameters bcPriKey = new RsaPrivateCrtKeyParameters(
                new BigInteger(1, priKey.Modulus),
                new BigInteger(1, priKey.Exponent),
                new BigInteger(1, priKey.D),
                new BigInteger(1, priKey.P),
                new BigInteger(1, priKey.Q),
                new BigInteger(1, priKey.DP),
                new BigInteger(1, priKey.DQ),
                new BigInteger(1, priKey.InverseQ));

            PrivateKeyInfo privateKeyInfo = PrivateKeyInfoFactory.CreatePrivateKeyInfo(bcPriKey);
            Asn1Object asn1ObjectPrivate = privateKeyInfo.ToAsn1Object();
            byte[] privateInfoByte = asn1ObjectPrivate.GetEncoded();

            return Convert.ToBase64String(privateInfoByte);
        }

        public static string PrivateKeyBase64ToXml(string base64PrivateKey)
        {
            RsaPrivateCrtKeyParameters priKey =
                (RsaPrivateCrtKeyParameters)
                    PrivateKeyFactory.CreateKey(Org.BouncyCastle.Utilities.Encoders.Base64.Decode(base64PrivateKey));

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

            var rsa = new RSACryptoServiceProvider();
            rsa.ImportParameters(p);

            return rsa.ToXmlString(true);
        }

        public static string PublicKeyXmlToBase64(string xmlPublicKey)
        {
            var rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(xmlPublicKey);

            var pubKey = rsa.ExportParameters(false);

            var bcPubKey = new RsaKeyParameters(false,
                new BigInteger(1, pubKey.Modulus),
                new BigInteger(1, pubKey.Exponent)
                );

            SubjectPublicKeyInfo subjectPublicKeyInfo = SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo(bcPubKey);
            Asn1Object asn1ObjectPublic = subjectPublicKeyInfo.ToAsn1Object();
            byte[] publicInfoByte = asn1ObjectPublic.GetEncoded();

            return Convert.ToBase64String(publicInfoByte);
        }

        public static string PublicKeyBase64ToXml(string base64PublicKey)
        {
            RsaKeyParameters pubKey =
                (Org.BouncyCastle.Crypto.Parameters.RsaKeyParameters)
                    PublicKeyFactory.CreateKey(Org.BouncyCastle.Utilities.Encoders.Base64.Decode(base64PublicKey));

            var p = new RSAParameters
            {
                Modulus = pubKey.Modulus.ToByteArrayUnsigned(),
                Exponent = pubKey.Exponent.ToByteArrayUnsigned(),
            };

            var rsa = new RSACryptoServiceProvider();
            rsa.ImportParameters(p);

            return rsa.ToXmlString(false);
        }

        #endregion XML与Base64转换

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

        /// <summary>
        /// 获取php生成的私钥
        /// </summary>
        /// <param name="base64PriKey"></param>
        private static RsaKeyParameters GetPhpPrivateKey(string base64PriKey)
        {
            Stream stream = new MemoryStream(Org.BouncyCastle.Utilities.Encoders.Base64.Decode(base64PriKey));

            var privKeyObj = Asn1Object.FromStream(stream);
            var privStruct = new RsaPrivateKeyStructure((Asn1Sequence)privKeyObj);
            RsaKeyParameters priParameters = new RsaPrivateCrtKeyParameters(
                new BigInteger(1, privStruct.Modulus.ToByteArrayUnsigned()),
                new BigInteger(1, privStruct.PublicExponent.ToByteArrayUnsigned()),
                new BigInteger(1, privStruct.PrivateExponent.ToByteArrayUnsigned()),
                new BigInteger(1, privStruct.Prime1.ToByteArrayUnsigned()),
                new BigInteger(1, privStruct.Prime2.ToByteArrayUnsigned()),
                new BigInteger(1, privStruct.Exponent1.ToByteArrayUnsigned()),
                new BigInteger(1, privStruct.Exponent2.ToByteArrayUnsigned()),
                new BigInteger(1, privStruct.Coefficient.ToByteArrayUnsigned())
                );

            return priParameters;
        }
    }
}