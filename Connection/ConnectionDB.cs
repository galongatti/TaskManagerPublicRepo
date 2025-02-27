using Microsoft.Extensions.Configuration;
using Npgsql;
using TaskManagerBackEnd.Config;

namespace TaskManagerBackEnd.Connection;

public class ConnectionDB : IDisposable
{
    private readonly SecretsManager _secretsManager;
    private NpgsqlConnection? _sqlConn;

    public ConnectionDB(SecretsManager secretsManager)
    {
        _secretsManager = secretsManager;
    }

    public NpgsqlConnection OpenConnection()
    {
        string? connectionString = _secretsManager.GetConnectionStringAsync().Result;
        _sqlConn = new NpgsqlConnection(connectionString);
        _sqlConn.Open();
        return _sqlConn;
    }

    public void Dispose()
    {
        _sqlConn?.Close();
        GC.SuppressFinalize(this);
    }
}