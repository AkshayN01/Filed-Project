using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Filed.PaymentGateway.Library.Encryption
{
    public class RSAEncryption
    {
        private static readonly Int64 _KeySize = 4096;

        public String EncryptAndSave(string plainText)
        {
            String cipherText;

            using (var rsa = GetRSACryptoProvider())
            {
                var plainTextBytes = Encoding.Unicode.GetBytes(plainText);
                var cipherTextBytes = rsa.Encrypt(plainTextBytes, RSAEncryptionPadding.Pkcs1);
                cipherText = Convert.ToBase64String(cipherTextBytes);   
            }

            return cipherText;
        }

        public String LoadAndDecrypt(string cipherText)
        {
            var plainText = string.Empty;

            using (var rsa = GetRSACryptoProvider())
            {
                var cipherTextBytes = Convert.FromBase64String(cipherText);
                var plainTextBytes = rsa.Decrypt(cipherTextBytes, RSAEncryptionPadding.Pkcs1);
                plainText = Encoding.Unicode.GetString(plainTextBytes);
            }

            return plainText;
        }

        private RSA GetRSACryptoProvider()
        {
            try
            {
                var rsa = RSA.Create();
                rsa.KeySize = Convert.ToInt32(_KeySize);
                return rsa;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in GetRSACryptoProvider(): {ex}");
                return null;
            }
        }
    }
}
