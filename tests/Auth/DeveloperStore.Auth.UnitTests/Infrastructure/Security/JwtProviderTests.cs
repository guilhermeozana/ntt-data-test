using System.IdentityModel.Tokens.Jwt;
using System.Text;
using DeveloperStore.Auth.Contracts.Security;
using DeveloperStore.Auth.Infrastructure.Security;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Xunit;

namespace DeveloperStore.Auth.UnitTests.Infrastructure.Security;

public class JwtProviderTests
{
    [Fact]
    public void Generate_ShouldReturnValidToken_WhenPayloadIsCorrect()
    {
        // Arrange
        var payload = new TokenPayload
        {
            UserId = 1,
            Username = "guilherme",
            Email = "gui@example.com",
            Role = "Customer"
        };

        var configValues = new Dictionary<string, string?>
        {
            { "Jwt:Secret", "super_secure_test_secret_key_1234567890" },
            { "Jwt:Issuer", "DeveloperStore" },
            { "Jwt:Audience", "DeveloperAudience" }
        };

        IConfiguration config = new ConfigurationBuilder()
            .AddInMemoryCollection(configValues!)
            .Build();

        var provider = new JwtProvider(config);

        // Act
        var token = provider.Generate(payload);

        // Assert
        token.Should().NotBeNullOrWhiteSpace();

        var handler = new JwtSecurityTokenHandler();
        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = config["Jwt:Issuer"],
            ValidateAudience = true,
            ValidAudience = config["Jwt:Audience"],
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Secret"]!)),
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };

        handler.ValidateToken(token, validationParameters, out var validatedToken);

        validatedToken.Should().BeOfType<JwtSecurityToken>();
        var jwt = (JwtSecurityToken)validatedToken;

        jwt.Claims.Should().Contain(c => c.Type == JwtRegisteredClaimNames.Sub && c.Value == payload.UserId.ToString());
        jwt.Claims.Should().Contain(c => c.Type == JwtRegisteredClaimNames.Email && c.Value == payload.Email);
        jwt.Claims.Should().Contain(c => c.Type == JwtRegisteredClaimNames.UniqueName && c.Value == payload.Username);
        jwt.Claims.Should().Contain(c => c.Type == System.Security.Claims.ClaimTypes.Role && c.Value == payload.Role);
    }
}