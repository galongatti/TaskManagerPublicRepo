using Npgsql;
using src.TaskManagerBackEnd.Config;

namespace src.TaskManagerBackEnd.Connection;

public class ConnectionDB : IDisposable
{
    private readonly SecretsManager _secretsManager;
    private NpgsqlConnection? _sqlConn;

    public ConnectionDB(SecretsManager secretsManager)
    {
        _secretsManager = secretsManager;
    }

    public void Dispose()
    {
        _sqlConn?.Close();
        GC.SuppressFinalize(this);
    }

    public NpgsqlConnection OpenConnection()
    {
        string? connectionString = _secretsManager.GetConnectionStringAsync().Result;
        _sqlConn = new NpgsqlConnection(connectionString);
        _sqlConn.Open();
        return _sqlConn;
    }
}