using src.TaskManagerBackEnd.Repository;
using TaskManagerBackEnd.Config;
using TaskManagerBackEnd.Connection;
using TaskManagerBackEnd.Repository;
using TaskManagerBackEnd.Service;

namespace TaskManagerBackEnd;

public static class DependencyInjectionConfig
{
    public static void AddCustomServices(this IServiceCollection services)
    {
        services.AddTransient<IUserService, UserService>();
        services.AddTransient<IUserRepository, UserRepository>(); 
        
        services.AddTransient<ITeamService, TeamService>();
        services.AddTransient<ITeamRepository, TeamRepository>();
        
        services.AddSingleton<SecretsManager>();
        services.AddSingleton<ConnectionDb>();
    }
}