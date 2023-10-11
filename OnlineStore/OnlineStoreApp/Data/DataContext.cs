using System.Data;
using Dapper;
using Microsoft.Extensions.Options;
using Npgsql;

namespace OnlineStoreApp.Data
{
    public class DataContext
    {
        private DbSettings _dbSettings;

    public DataContext(IOptions<DbSettings> dbSettings)
    {
        _dbSettings = dbSettings.Value;
    }

    public IDbConnection CreateConnection()
    {
        var connectionString = $"Host={_dbSettings.Server}; Database={_dbSettings.Database}; Username={_dbSettings.UserId}; Password={_dbSettings.Password};";
        return new NpgsqlConnection(connectionString);
    }

    public async Task Init()
    {
        await InitDatabase();
    }

    private async Task InitDatabase()
    {
        // create database if it doesn't exist
        var connectionString = $"Host={_dbSettings.Server}; Database=postgres; Username={_dbSettings.UserId}; Password={_dbSettings.Password};";
        using var connection = new NpgsqlConnection(connectionString);
        var sqlDbCount = $"SELECT COUNT(*) FROM pg_database WHERE datname = '{_dbSettings.Database}';";
        var dbCount = await connection.ExecuteScalarAsync<int>(sqlDbCount);
        if (dbCount == 0)
        {
            var sql = $"CREATE DATABASE \"{_dbSettings.Database}\"";
            await connection.ExecuteAsync(sql);
            await InitTables();
        }
    }

    private async Task InitTables()
    {
        // create tables if they don't exist
        using var connection = CreateConnection();
        await InitProducts();
        await SeedProducts();

        async Task InitProducts()
        {
            var sql = """
                CREATE TABLE IF NOT EXISTS products (
                id SERIAL PRIMARY KEY NOT NULL, 
                name VARCHAR(100) NOT NULL, 
                price MONEY NOT NULL);
            """;
            await connection.ExecuteAsync(sql);
        }

        async Task SeedProducts()
        {
            var sql = """
            INSERT INTO products (Name, Price) 
            VALUES  ('Phone', 299.99), 
                    ('Notebook', 599.99),
                    ('Laptop', 899.99)
            """;
            await connection.ExecuteAsync(sql);
        }
    }
    }
}