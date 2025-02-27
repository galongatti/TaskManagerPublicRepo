using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace TaskManagerBackEnd.Config;

public class SecretsManager
{
    private readonly IAmazonSecretsManager _secretsManager;
    private readonly IConfiguration _configuration;

    public SecretsManager(IAmazonSecretsManager secretsManager, IConfiguration configuration)
    {
        _secretsManager = secretsManager;
        _configuration = configuration;
    }

    public async Task<string> GetSecretValueAsync(string secretName)
    {
        var request = new GetSecretValueRequest
        {
            SecretId = secretName
        };

        var response = await _secretsManager.GetSecretValueAsync(request);
        return response.SecretString;
    }

    public async Task<string> GetConnectionStringAsync()
    {
        string enviroment = _configuration["Environment"];
        
        if(enviroment == "Development")
        {
            string connectionString = _configuration["TaskManager:ConnectionString"];
            return connectionString;
        }
        
        var secretName = _configuration["Secrets:ConnectionString"];
        var secretValue = await GetSecretValueAsync(secretName);
        var secret = JsonSerializer.Deserialize<Dictionary<string, string>>(secretValue);
        return secret["ConnectionString"];
    }
}