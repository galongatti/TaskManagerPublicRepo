using Npgsql;
using TaskManagerBackEnd.Config;

namespace TaskManagerBackEnd.Connection;

public class ConnectionDb : IDisposable
{
    private readonly SecretsManager _secretsManager;
    private NpgsqlConnection? _sqlConn;

    public ConnectionDb(SecretsManager secretsManager)
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