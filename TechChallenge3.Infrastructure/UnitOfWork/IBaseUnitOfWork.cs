using System.Transactions;

namespace TechChallenge3.Infrastructure.UnitOfWork
{
    public interface IBaseUnitOfWork : IDisposable
    {
        ITransaction BeginTransaction();
        ITransaction BeginTransaction(TransactionOptions transactionOptions);
    }
}
