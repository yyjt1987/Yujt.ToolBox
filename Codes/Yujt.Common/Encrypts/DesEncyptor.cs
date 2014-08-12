using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Yujt.Common.Encrypts
{
    public class DesEncyptor
    {
        private const string KEY = "UIOfLeJN";
        public static string Encrypt(string encryptingStr, string encodingStr = "UTF-8")
        {
            var encoding = Encoding.GetEncoding(encodingStr);
            var key = encoding.GetBytes(KEY);
            var desCryptoServiceProvider = new DESCryptoServiceProvider();
            using (var memoryStream = new MemoryStream())
            {
                var cryptoStream = new CryptoStream(memoryStream,
                                                    desCryptoServiceProvider.CreateEncryptor(key, key),
                                                    CryptoStreamMode.Write);
                using (cryptoStream)
                {
                    var encryptingBytes = encoding.GetBytes(encryptingStr);
                    cryptoStream.Write(encryptingBytes, 0, encryptingBytes.Length);
                    cryptoStream.FlushFinalBlock();

                    return Convert.ToBase64String(memoryStream.ToArray());
                }
            }
        }

        public static string Decrypt(string encryptedStr, string encodingStr = "UTF-8")
        {
            var encoding = Encoding.GetEncoding(encodingStr);
            var key = encoding.GetBytes(KEY);
            var desCryptoServiceProvider = new DESCryptoServiceProvider();
            var encryptedBytes = Convert.FromBase64String(encryptedStr);
            using (var memoryStream = new MemoryStream(encryptedBytes))
            {
                var cryptoStream = new CryptoStream(memoryStream,
                    desCryptoServiceProvider.CreateDecryptor(key, key),
                    CryptoStreamMode.Read);

                using (cryptoStream)
                {
                    var reader = new StreamReader(cryptoStream, encoding);

                    return reader.ReadToEnd();
                }
            }
        }

    }
}
