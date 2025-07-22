using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SalesRecords.Auth.Application.Security;
using SalesRecords.Auth.Contracts.Security;

namespace SalesRecords.Auth.Infrastructure.Security;

public class JwtProvider : IJwtProvider
{
    private readonly IConfiguration _config;

    public JwtProvider(IConfiguration config)
    {
        _config = config;
    }

    public string Generate(TokenPayload payload)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, payload.UserId.ToString()),
            new Claim(JwtRegisteredClaimNames.UniqueName, payload.Username),
            new Claim(JwtRegisteredClaimNames.Email, payload.Email),
            new Claim(ClaimTypes.Role, payload.Role),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Secret"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}