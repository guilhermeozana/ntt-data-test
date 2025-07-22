using FluentValidation;
using MediatR;
using SalesRecords.Carts.Api.Mapping;
using SalesRecords.Carts.Application;
using SalesRecords.Carts.Application.Behaviors;
using SalesRecords.Carts.Application.Commands.CreateCart;
using SalesRecords.Carts.Infrastructure;
using SalesRecords.Shared.Api.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddAutoMapper(
    cfg => { },
    typeof(CartMappingProfile)
);
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddValidatorsFromAssemblyContaining<CreateCartCommand>();
builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.MapControllers();
app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseAuthentication();
app.UseAuthorization();


app.Run();