using Microsoft.Extensions.Configuration;
using TechChallenge3.Infrastructure.Crypto;

namespace TechChallenge3.Infrastructure.UnitOfWork
{
    public class TechDatabase : BaseConnection, ITechDatabase
    {
        public TechDatabase(IConfiguration configuration, ICryptoService cryptoService)
            : base(configuration ?? throw new ArgumentNullException(nameof(configuration)),
                  cryptoService ?? throw new ArgumentNullException(nameof(cryptoService)))
        { }
    }
}
