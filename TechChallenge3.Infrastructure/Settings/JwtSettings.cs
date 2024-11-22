namespace TechChallenge3.Infrastructure.Settings
{
    public class JwtSettings
    {
        private const int DEFAULT_TOKEN_EXPIRES_MINUTES = 5;

        public JwtSettings(int tokenExpiresInMinutes = DEFAULT_TOKEN_EXPIRES_MINUTES) =>
            TokenExpiresInMinutes = tokenExpiresInMinutes;

        public string SecretKey { get; set; }
        public int TokenExpiresInMinutes { get; set; }
    }
}