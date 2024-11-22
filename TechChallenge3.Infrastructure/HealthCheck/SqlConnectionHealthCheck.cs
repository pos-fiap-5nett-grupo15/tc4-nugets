using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using TechChallenge3.Infrastructure.Crypto;
using TechChallenge3.Infrastructure.UnitOfWork;

namespace TechChallenge3.Infrastructure.HealthCheck
{
    public class SqlConnectionHealthCheck : BaseConnection, IHealthCheck
    {
        private string _healthQuery;

        public SqlConnectionHealthCheck(IConfiguration configuration, ICryptoService cryptoService, string heathQuery = "SELECT 1") : base(configuration, cryptoService)
        {
            this._healthQuery = heathQuery;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken)
        {
            try
            {
                using (var command = this.InitializeConnection().ExecuteScalarAsync(new CommandDefinition(_healthQuery, cancellationToken)))
                {
                    command.Wait();
                }
                return HealthCheckResult.Healthy("SQL Server is healthy.");
            }
            catch (Exception ex)
            {
                return HealthCheckResult.Unhealthy("SQL Server is unhealthy.", ex);
            }
        }
    }
}
