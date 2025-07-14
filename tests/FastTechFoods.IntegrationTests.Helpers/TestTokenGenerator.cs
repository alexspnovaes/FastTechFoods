using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace FastTechFoods.IntegrationTests.Helpers;

public static class TestTokenGenerator
{
    private const string Key = "my_super_secret_key_1234567890abcd";
    private const string Issuer = "FastTechFoods.Auth";
    private const string Audience = "FastTechFoods.Client";

    public static string GenerateJwtToken(Guid clientId, string role = "client")
    {
        var key = Encoding.UTF8.GetBytes(Key);
        var creds = new SigningCredentials(
                         new SymmetricSecurityKey(key),
                         SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, clientId.ToString()),
            new Claim(ClaimTypes.Role, role)
        };

        var token = new JwtSecurityToken(
            issuer: Issuer,
            audience: Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
