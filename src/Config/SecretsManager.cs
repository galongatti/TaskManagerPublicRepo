using System.Text.Json;
using Amazon;
using Amazon.Runtime;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;

namespace TaskManagerBackEnd.Config;

public class SecretsManager
{
    private readonly IConfiguration _configuration;
    private readonly IAmazonSecretsManager _secretsManager;

    public SecretsManager(IConfiguration configuration, IAmazonSecretsManager secretsManager)
    {
        _configuration = configuration;
        _secretsManager = secretsManager;
    }

    public string GetSecretValueAsync(string secretName)
    {
        GetSecretValueRequest request = new()
        {
            SecretId = secretName
        };

        GetSecretValueResponse response = _secretsManager.GetSecretValueAsync(request).Result;
        return response.SecretString;
    }

  
}