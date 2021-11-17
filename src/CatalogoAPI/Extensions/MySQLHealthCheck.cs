using Microsoft.Extensions.Diagnostics.HealthChecks;
using MySqlConnector;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CatalogoAPI.Extensions
{
    public class MySQLHealthCheck : IHealthCheck
    {
        readonly string _connection;

        public MySQLHealthCheck(string connection)
        {
            _connection = connection;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                using (var connection = new MySqlConnection(_connection))
                {
                    await connection.OpenAsync(cancellationToken);

                    var command = connection.CreateCommand();
                    command.CommandText = "select count(ProdutoId) from Produtos";

                    var data = Convert.ToInt32(await command.ExecuteScalarAsync(cancellationToken));

                    return await Task.FromResult(new HealthCheckResult(
                                status: data > 0 ? HealthStatus.Healthy : HealthStatus.Unhealthy,
                                description: data > 0 ? "Existe produtos cadastrados" : "Nenhum produto encontrado."
                                ));
                }
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new HealthCheckResult(
                                status: HealthStatus.Unhealthy,
                                description: ex.StackTrace
                                ));
            }
        }
    }
}
