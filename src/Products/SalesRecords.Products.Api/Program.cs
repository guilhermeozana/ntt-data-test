using FluentValidation;
using MediatR;
using SalesRecords.Products.Api.Mapping;
using SalesRecords.Products.Application;
using SalesRecords.Products.Application.Behaviors;
using SalesRecords.Products.Application.Commands.CreateProduct;
using SalesRecords.Products.Infrastructure;
using SalesRecords.Shared.Api.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddAutoMapper(
    cfg => { },
    typeof(ProductMappingProfile)
);
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddValidatorsFromAssemblyContaining<CreateProductCommand>();
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