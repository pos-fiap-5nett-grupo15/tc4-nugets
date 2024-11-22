using System.Transactions;

namespace TechChallenge3.Infrastructure.UnitOfWork
{
    public class BaseUnitOfWork : IBaseUnitOfWork
    {
        private readonly ITechDatabase _techDabase;

        public BaseUnitOfWork(ITechDatabase database)
        {
            this._techDabase = database;
        }

        #region Transaction methods:
        public ITransaction BeginTransaction()
        {
            var tx = new Transaction();
            this._techDabase.EnsureConnectionIdOpen();
            return tx;
        }

        public ITransaction BeginTransaction(System.Transactions.TransactionOptions transactionOptions)
        {
            var tx = new Transaction(transactionOptions);
            this._techDabase.EnsureConnectionIdOpen();
            return tx;
        }
        #endregion Transaction methods.

        #region IDispose Support
        public bool disposidedValue = false;

        void Dispose(bool disposing)
        {
            if (disposidedValue)
            {
                this._techDabase.Dispose();
            }
            disposidedValue = true;
        }

        public void Dispose() =>
            this.Dispose(true);
        #endregion
    }
}
