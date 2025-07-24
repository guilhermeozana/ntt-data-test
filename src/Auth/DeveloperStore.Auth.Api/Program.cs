using DeveloperStore.Auth.Api.Mapping;
using DeveloperStore.Auth.Application;
using DeveloperStore.Auth.Infrastructure;
using DeveloperStore.Shared.Api.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddAutoMapper(
    cfg => { },
    typeof(AuthMappingProfile)
);
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new()
    {
        Title = "DeveloperStore Auth API",
        Version = "v1",
        Description = "JWT Authentication for DeveloperStore."
    });
});   

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.UseMiddleware<ErrorHandlingMiddleware>();

app.Run();