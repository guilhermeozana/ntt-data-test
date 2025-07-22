using FluentValidation;
using MediatR;
using SalesRecords.Carts.Infrastructure;
using SalesRecords.Shared.Api.Middlewares;
using SalesRecords.Users.Api.Mapping;
using SalesRecords.Users.Application;
using SalesRecords.Users.Application.Behaviors;
using SalesRecords.Users.Application.Commands.CreateUser;
using SalesRecords.Users.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddAutoMapper(
    cfg => { },
    typeof(UserMappingProfile)
);
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddValidatorsFromAssemblyContaining<CreateUserCommand>();
builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.MapControllers();
app.UseMiddleware<ErrorHandlingMiddleware>();

app.Run();