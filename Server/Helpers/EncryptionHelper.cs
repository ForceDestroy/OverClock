namespace Server.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Text;
    using System.Security.Cryptography;

    namespace Server.Helpers
    {
        public static class EncryptionHelper
        {
            public static readonly string ParameterKey = System.IO.File.ReadAllText("EncryptionKey.txt");
            public static readonly string TLSKey = System.IO.File.ReadAllText("TLSKey.txt");

            public static string EncryptString(string plainText)
            {
                return Encrypt(plainText, ParameterKey);
            }

            public static string DecryptString(string plainText)
            {
                return Decrypt(plainText, ParameterKey);
            }

            public static string EncryptTLS(string plainText)
            {
                return Encrypt(plainText, TLSKey);
            }

            public static string DecryptTLS(string plainText)
            {
                return Decrypt(plainText, TLSKey);
            }

            private static string Encrypt(string plainText, string key)
            {
                byte[] iv = Encoding.UTF8.GetBytes("0000000000000000");
                byte[] array;

                using (Aes aes = Aes.Create())
                {
                    aes.Key = Encoding.UTF8.GetBytes(key);
                    aes.IV = iv;
                    aes.Mode = CipherMode.CBC;
                    aes.Padding = PaddingMode.PKCS7;

                    ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                    using MemoryStream memoryStream = new();
                    using CryptoStream cryptoStream = new((Stream)memoryStream, encryptor, CryptoStreamMode.Write);
                    using (StreamWriter streamWriter = new((Stream)cryptoStream))
                    {
                        streamWriter.Write(plainText);
                    }

                    array = memoryStream.ToArray();
                }

                return Convert.ToBase64String(array);
            }

            public static string Decrypt(string cipherText, string key)
            {
                byte[] iv = Encoding.UTF8.GetBytes("0000000000000000");
                byte[] buffer = Convert.FromBase64String(cipherText);

                using Aes aes = Aes.Create();
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;

                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using MemoryStream memoryStream = new(buffer);
                using CryptoStream cryptoStream = new((Stream)memoryStream, decryptor, CryptoStreamMode.Read);
                using StreamReader streamReader = new((Stream)cryptoStream);

                return streamReader.ReadToEnd();
            }
        }
    }
}