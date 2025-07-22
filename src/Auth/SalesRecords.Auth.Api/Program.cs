using FluentValidation;
using MediatR;
using SalesRecords.Auth.Api.Mapping;
using SalesRecords.Auth.Application;
using SalesRecords.Auth.Application.Behaviors;
using SalesRecords.Auth.Application.Commands.Login;
using SalesRecords.Auth.Application.Security;
using SalesRecords.Auth.Infrastructure;
using SalesRecords.Auth.Infrastructure.Security;
using SalesRecords.Shared.Api.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddAutoMapper(
    cfg => { },
    typeof(AuthMappingProfile)
);
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddValidatorsFromAssemblyContaining<LoginCommandValidator>();
builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
builder.Services.AddScoped<IJwtProvider, JwtProvider>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.MapControllers();
app.UseMiddleware<ErrorHandlingMiddleware>();

app.Run();