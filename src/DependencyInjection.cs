using src.TaskManagerBackEnd.Config;
using src.TaskManagerBackEnd.Connection;
using src.TaskManagerBackEnd.Repository;
using src.TaskManagerBackEnd.Service;

namespace src.TaskManagerBackEnd;

public static class DependencyInjectionConfig
{
    public static IServiceCollection AddCustomServices(this IServiceCollection services)
    {
        services.AddTransient<IMemberService, MemberService>();
        services.AddTransient<IUserRepository, UserRepository>();
        services.AddSingleton<SecretsManager>();
        services.AddSingleton<ConnectionDB>();
        return services;
    }
}