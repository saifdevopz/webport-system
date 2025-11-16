using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Data.Common;
using WebportSystem.Common.Application.Database;

namespace WebportSystem.Common.Infrastructure.Database;

internal sealed class DbConnectionFactory(IConfiguration configuration) : IDbConnectionFactory
{
    private string GetConnectionString()
    {
        string provider = configuration["Database:ActiveProvider"]
            ?? throw new ArgumentException("Missing Database:ActiveProvider in configuration.");

        string basePath = $"Database:Providers:{provider}";

        // Fetch connection strings dynamically
        return configuration[$"{basePath}:IdentityConnection"]!;
    }

    public async ValueTask<DbConnection> OpenTenantConnection(string? connectionString = null)
    {

        NpgsqlConnection connection = new(GetConnectionString());

        await connection.OpenAsync();

        return connection;
    }

    public async ValueTask<DbConnection> OpenIdentityConnection()
    {
        NpgsqlConnection connection = new(GetConnectionString());

        await connection.OpenAsync();

        return connection;
    }

    public async Task<List<T>> QueryAsync<T>(string sql, object parameters = null!, bool systemDb = false)
    {
        using DbConnection connection = await OpenIdentityConnection();

        IEnumerable<T> result = await connection.QueryAsync<T>(sql, parameters);
        return [.. result];
    }
}