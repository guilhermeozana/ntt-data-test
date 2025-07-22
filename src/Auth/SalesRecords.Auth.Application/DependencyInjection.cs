using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using SalesRecords.Auth.Application.Security;

namespace SalesRecords.Auth.Application;

public static class  DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddFluentValidationAutoValidation(); 
        services.AddMediatR(options =>
        {
            options.RegisterServicesFromAssemblyContaining(typeof(DependencyInjection));
        });
    
        services.AddValidatorsFromAssemblyContaining(typeof(DependencyInjection));

        return services;
    }
}