using System.Data;

namespace TechChallenge3.Infrastructure.UnitOfWork
{
    public interface ITechDatabase : IDisposable
    {
        IDbConnection Connection { get; }

        void EnsureConnectionIdOpen();
    }
}
