using System.Security.Cryptography;
using TechChallenge3.Infrastructure.Settings;

namespace TechChallenge3.Infrastructure.Crypto
{
    public class CryptoService : ICryptoService
    {
        private readonly byte[] _key;
        private readonly byte[] _iv;

        public CryptoService(CryptoSettings? cryptoSettings)
        {
            var secKey = cryptoSettings?.SecKey ?? Environment.GetEnvironmentVariable("TECH1_API_SEC_KEY");
            var secIv = cryptoSettings?.SecIv ?? Environment.GetEnvironmentVariable("TECH1_API_SEC_IV");
            ArgumentNullException.ThrowIfNull(secKey);
            ArgumentNullException.ThrowIfNull(secIv);

            _key = Convert.FromBase64String(secKey);
            _iv = Convert.FromBase64String(secIv);
        }

        public string Decrypt(string cipherText)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = _key;
                aesAlg.IV = _iv;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(cipherText)))
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                    {
                        return srDecrypt.ReadToEnd();
                    }
                }
            }
        }

        public string Encrypt(string plainText)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = _key;
                aesAlg.IV = _iv;

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(plainText);
                    }
                    return Convert.ToBase64String(msEncrypt.ToArray());
                }
            }
        }

        public (string, string) GenerateCryptKeys()
        {
            var key = string.Empty;
            var iv = string.Empty;

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.GenerateKey();
                aesAlg.GenerateIV();

                key = Convert.ToBase64String(aesAlg.Key);
                iv = Convert.ToBase64String(aesAlg.IV);
            }

            return (key, iv);
        }
    }
}
