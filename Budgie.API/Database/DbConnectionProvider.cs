using System.Data.Common;
using Npgsql;

namespace Budgie.API.Database;

public sealed class DbConnectionProvider(string connectionString)
{
    private readonly string _connectionString = connectionString;

    public DbConnection GetConnection()
    {
        return new NpgsqlConnection(_connectionString);
    }
}