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

    public string? GetConnectionStringAsync()
    {
        string enviroment = _configuration["Environment"] ??
                            throw new ArgumentNullException("Environmet variable not found");
        
        string userSecret = _configuration["ConnectionDB:User"] ?? throw new ArgumentNullException("User not found");
        string user = GetSecretValueAsync(userSecret);
        Dictionary<string, string>? secretDictionary = JsonSerializer.Deserialize<Dictionary<string, string>>(user);

        if (enviroment.Equals("development"))
        {
            return $"Server=localhost:3309;Database=TaskManager;User Id={secretDictionary["username"]};Password={secretDictionary["password"]};";
        }
        else
        {
           
            string endpointSecret = _configuration["ConnectionDB:Endpoint"] ??
                                    throw new ArgumentNullException("Endpoint not found");

            string endpoint = GetSecretValueAsync(endpointSecret);
            Dictionary<string, string>? secretHostDictionary =
                JsonSerializer.Deserialize<Dictionary<string, string>>(endpoint);

            return $"Server={secretHostDictionary["Endpoint"]};Database=TaskManager;User Id={secretDictionary["username"]};Password={secretDictionary["password"]};";
        }
    }
}