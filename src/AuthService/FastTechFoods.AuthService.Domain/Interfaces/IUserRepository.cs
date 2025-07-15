using FastTechFoods.AuthService.Domain.Entities;

namespace FastTechFoods.AuthService.Domain.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByCpfAsync(string cpf);
    Task AddAsync(User user);
}
