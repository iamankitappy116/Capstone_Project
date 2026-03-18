using System;
using System.Security.Cryptography;
using System.Text;

namespace SecureUserApp.Utils
{
    public static class EncryptionHelper
    {
        private static readonly byte[] AesKey =
            Encoding.UTF8.GetBytes("MySecretKey12345");

        private static readonly byte[] AesIV =
            Encoding.UTF8.GetBytes("MyInitVector1234");

        public static string EncryptData(string plainText)
        {
            try
            {
                using (Aes aes = Aes.Create())
                {
                    aes.Key = AesKey;
                    aes.IV = AesIV;

                    var encryptor = aes.CreateEncryptor();
                    byte[] inputBytes = Encoding.UTF8.GetBytes(plainText);
                    byte[] encryptedBytes =
                        encryptor.TransformFinalBlock(inputBytes, 0, inputBytes.Length);

                    return Convert.ToBase64String(encryptedBytes);
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Encryption failed.", ex);
            }
        }

        public static string DecryptData(string encryptedText)
        {
            try
            {
                using (Aes aes = Aes.Create())
                {
                    aes.Key = AesKey;
                    aes.IV = AesIV;

                    var decryptor = aes.CreateDecryptor();
                    byte[] encryptedBytes =
                        Convert.FromBase64String(encryptedText);

                    byte[] decryptedBytes =
                        decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);

                    return Encoding.UTF8.GetString(decryptedBytes);
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Decryption failed.", ex);
            }
        }
    }
}