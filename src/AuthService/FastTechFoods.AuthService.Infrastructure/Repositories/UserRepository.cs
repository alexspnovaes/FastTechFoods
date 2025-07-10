using FastTechFoods.AuthService.Domain.Entities;
using FastTechFoods.AuthService.Domain.Interfaces;
using FastTechFoods.AuthService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FastTechFoods.AuthService.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AuthDbContext _context;

    public UserRepository(AuthDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
    }

    public async Task<User?> GetByEmailAsync(string email)
        => await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

    public async Task<User?> GetByCpfAsync(string cpf)
        => await _context.Users.FirstOrDefaultAsync(u => u.CPF == cpf);
}
