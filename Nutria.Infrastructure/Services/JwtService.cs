using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Nutria.Domain.Interfaces;
using Nutria.Domain.Interfaces.Services;

namespace Nutria.Infrastructure.Services;

public class JwtService : IJwtService
{
    private readonly IConfiguration _configuration;

    public JwtService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    //public string GenerateJwtToken(string userId, string userName, string role)
    public string GenerateJwtToken(string userId, string userName)
    {
        // 1. Crear los Claims (datos dentro del token)
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId),
            new Claim(JwtRegisteredClaimNames.UniqueName, userName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            //new Claim(ClaimTypes.Role, role)
        };

        // 2. Obtener la llave secreta que usaste en ServiceRegistrationExtensions.cs
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        // 3. Crear el token con los validadores (Issuer y Audience)
        var token = new JwtSecurityToken(
            issuer: "languagebridgesolutions.com",
            audience: "languagebridgesolutions.com",
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}