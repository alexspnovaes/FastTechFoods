using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace FastTechFoods.BuildingBlocks.Security;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration config)
    {
        var jwt = config.GetSection("Jwt");
        var keyBytes = Encoding.UTF8.GetBytes(jwt["Key"]!);

        services
          .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
          .AddJwtBearer(options =>
          {
              options.TokenValidationParameters = new TokenValidationParameters
              {
                  ValidateIssuer = true,
                  ValidIssuer = jwt["Issuer"],

                  ValidateAudience = true,
                  ValidAudience = jwt["Audience"],

                  ValidateIssuerSigningKey = true,
                  IssuerSigningKey = new SymmetricSecurityKey(keyBytes),

                  ValidateLifetime = true
              };
          });

        services.AddAuthorization();

        return services;
    }
}