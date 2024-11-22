namespace TechChallenge3.Infrastructure.Crypto
{
    public interface ICryptoService
    {
        public (string, string) GenerateCryptKeys();
        public string Encrypt(string plainText);
        public string Decrypt(string cipherText);
    }
}
