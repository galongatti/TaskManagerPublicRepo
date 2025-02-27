using TaskManagerBackEnd.Config;
using TaskManagerBackEnd.Connection;
using TaskManagerBackEnd.Repository;
using TaskManagerBackEnd.Service;

namespace TaskManagerBackEnd;

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