using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SalesRecords.Carts.Application.Common.Interfaces;
using SalesRecords.Carts.Infrastructure.Context;
using SalesRecords.Carts.Infrastructure.Repositories;

namespace SalesRecords.Carts.Infrastructure;

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
                    ValidIssuer = "SalesRecords.Auth",
                    ValidAudience = "SalesRecords.Client",
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(configuration["Jwt:Secret"]))
                };
            });
        
        return services;
    }
}