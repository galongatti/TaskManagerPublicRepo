using System.Text.Json;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;

namespace TaskManagerBackEnd.Config;

public class SecretsManager
{
    private readonly IConfiguration _configuration;
    private readonly IAmazonSecretsManager _secretsManager;

    public SecretsManager(IAmazonSecretsManager secretsManager, IConfiguration configuration)
    {
        _secretsManager = secretsManager;
        _configuration = configuration;
    }

    public async Task<string> GetSecretValueAsync(string secretName)
    {
        GetSecretValueRequest request = new()
        {
            SecretId = secretName
        };

        GetSecretValueResponse? response = await _secretsManager.GetSecretValueAsync(request);
        return response.SecretString;
    }

    public async Task<string?> GetConnectionStringAsync()
    {
        string? enviroment = _configuration["Environment"];

        if (enviroment == "Development")
        {
            string? connectionString = _configuration["TaskManager:ConnectionString"];
            return connectionString;
        }

        string? secretName = _configuration["Secrets:ConnectionString"] ??
                             throw new ArgumentNullException("Connection String secret name not found");
        string secretValue = await GetSecretValueAsync(secretName);
        Dictionary<string, string>? secret = JsonSerializer.Deserialize<Dictionary<string, string>>(secretValue);
        return secret["ConnectionString"];
    }
}