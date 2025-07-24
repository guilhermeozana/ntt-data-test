using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using DeveloperStore.Carts.Application.Common.Interfaces;
using DeveloperStore.Carts.Infrastructure.Context;
using DeveloperStore.Carts.Infrastructure.Repositories;

namespace DeveloperStore.Carts.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<CartsDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("CartsConnection")));

        services.AddScoped<ICartRepository, CartRepository>();
        
        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "DeveloperStore.Auth",
                    ValidAudience = "DeveloperStore.Client",
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(configuration["Jwt:Secret"]))
                };
            });
        
        return services;
    }
}