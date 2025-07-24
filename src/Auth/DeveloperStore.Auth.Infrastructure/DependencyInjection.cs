using DeveloperStore.Auth.Application.External;
using DeveloperStore.Auth.Application.Security;
using DeveloperStore.Auth.Infrastructure.External.Users;
using DeveloperStore.Auth.Infrastructure.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DeveloperStore.Auth.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient<IUsersApiClient, UsersApiClient>(client =>
        {
            client.BaseAddress = new Uri(configuration["UsersApi:BaseUrl"]);
        });
        services.AddScoped<IJwtProvider, JwtProvider>();
        return services;
    }
}