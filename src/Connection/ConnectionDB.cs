using System.Text.Json;
using Npgsql;
using TaskManagerBackEnd.Config;

namespace TaskManagerBackEnd.Connection;

public class ConnectionDb(IConfiguration configuration, SecretsManager secretsManager) : IDisposable
{
    private NpgsqlConnection? _sqlConn;

    public void Dispose()
    {
        _sqlConn?.Close();
        GC.SuppressFinalize(this);
    }

    public NpgsqlConnection OpenConnection()
    {
        string enviroment = configuration["Environment"] ??
                            throw new ArgumentNullException("Environmet variable not found");

        string? connectionString = string.Empty;


        if (enviroment.Equals("development"))
        {
            connectionString = configuration["localconnection"];
        }
        else
        {
            connectionString = GetConnectionStringDb();
        }


        _sqlConn = new NpgsqlConnection(connectionString);
        _sqlConn.Open();
        return _sqlConn;
    }

    private string? GetConnectionStringDb()
    {
        string userSecret = configuration["ConnectionDBUser"] ?? throw new ArgumentNullException("User not found");
        string endpointSecret = configuration["ConnectionDBEndpoint"] ?? throw new ArgumentNullException("Endpoint not found");
        
        string user = secretsManager.GetSecretValueAsync(userSecret);
        Dictionary<string, string>? secretDictionary = JsonSerializer.Deserialize<Dictionary<string, string>>(user);

        string endpoint = secretsManager.GetSecretValueAsync(endpointSecret);
        Dictionary<string, string>? secretHostDictionary =
            JsonSerializer.Deserialize<Dictionary<string, string>>(endpoint);

        return
            $"Server={secretHostDictionary["Endpoint"]};Database=TaskManager;User Id={secretDictionary["username"]};Password={secretDictionary["password"]};";
    }
}