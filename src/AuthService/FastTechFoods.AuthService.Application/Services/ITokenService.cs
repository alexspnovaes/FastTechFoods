using FastTechFoods.AuthService.Domain.Entities;

namespace FastTechFoods.AuthService.Application.Services;

public interface ITokenService
{
    string GenerateToken(User user);
}
