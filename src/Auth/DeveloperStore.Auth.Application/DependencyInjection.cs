using DeveloperStore.Auth.Application.Commands.Login;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using MediatR;
using DeveloperStore.Shared.Application.Behaviors;

namespace DeveloperStore.Auth.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddFluentValidationAutoValidation();

        services.AddMediatR(options =>
        {
            options.RegisterServicesFromAssemblyContaining(typeof(DependencyInjection));
        });
        services.AddValidatorsFromAssemblyContaining(typeof(DependencyInjection));
        
        services.AddValidatorsFromAssemblyContaining<LoginCommandValidator>();
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        return services;
    }
}