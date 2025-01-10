using System.Security.Cryptography;
using System.Text;
using System.IO;

namespace Budgie.API.Helpers
{
    public static class EncryptionHelper
    {
        public static string EncryptKey(string integrationKey, string secretKey)
        {
            var keyBytes = Convert.FromBase64String(secretKey);

            if (keyBytes.Length != 32)
            {
                Array.Resize(ref keyBytes, 32);
            }

            using var aes = Aes.Create();
            aes.Key = keyBytes;
            aes.GenerateIV();
            var iv = aes.IV;

            using var encryptor = aes.CreateEncryptor(aes.Key, iv);
            using var ms = new MemoryStream();
            using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
            {
                using var writer = new StreamWriter(cs);
                writer.Write(integrationKey);
            }

            var encryptedToken = Convert.ToBase64String(iv.Concat(ms.ToArray()).ToArray());
            return encryptedToken;
        }

        public static string DecryptKey(string encryptedIntegrationKey, string secretKey)
        {
            var fullCipher = Convert.FromBase64String(encryptedIntegrationKey);
            var iv = fullCipher.Take(16).ToArray();
            var cipher = fullCipher.Skip(16).ToArray();

            var keyBytes = Convert.FromBase64String(secretKey);
            if (keyBytes.Length != 32)
            {
                Array.Resize(ref keyBytes, 32);
            }

            using var aes = Aes.Create();
            aes.Key = keyBytes;
            aes.IV = iv;

            using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            using var ms = new MemoryStream(cipher);
            using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
            using var reader = new StreamReader(cs);
            return reader.ReadToEnd();
        }
    }
}
