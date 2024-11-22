using Microsoft.Extensions.Configuration;
using System.Data;
using TechChallenge3.Infrastructure.Crypto;

namespace TechChallenge3.Infrastructure.UnitOfWork
{
    public class BaseConnection : IDisposable
    {
        public string _connectonString { get; set; }

        private Lazy<IDbConnection> _connection;

        public IDbConnection Connection
        {
            get { return this._connection.Value; }
        }

        public BaseConnection(IConfiguration configuration, ICryptoService cryptoService)
        {
            var strconnection = configuration.GetConnectionString("DefaultConnection");

            if (string.IsNullOrEmpty(strconnection))
                throw new InvalidOperationException("DefaultConnection is not defined.");

            strconnection = cryptoService.Decrypt(strconnection);

            this._connectonString = strconnection;
            this._connection = new Lazy<IDbConnection>(InitializeConnection, true);
        }

        public IDbConnection InitializeConnection()
        {
            return new Microsoft.Data.SqlClient.SqlConnection(this._connectonString);
        }

        public void EnsureConnectionIdOpen()
        {
            if (this.Connection.State == ConnectionState.Broken) this.Connection.Close();

            if (this.Connection.State == ConnectionState.Closed) this.Connection.Open();
        }

        #region IDisposible support
        private bool _disposedValue = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    if (this._connection.IsValueCreated) this.Connection.Dispose();
                }
                this._disposedValue = true;
            }
        }

        public void Dispose() => this.Dispose(true);
        #endregion
    }
}
