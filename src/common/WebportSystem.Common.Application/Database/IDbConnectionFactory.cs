using System.Data.Common;

namespace WebportSystem.Common.Application.Database;

public interface IDbConnectionFactory
{
    ValueTask<DbConnection> OpenTenantConnection(string? connectionString = null);

    ValueTask<DbConnection> OpenIdentityConnection();

    Task<List<T>> QueryAsync<T>(string sql, object parameters = null!, bool systemDb = false);
}
