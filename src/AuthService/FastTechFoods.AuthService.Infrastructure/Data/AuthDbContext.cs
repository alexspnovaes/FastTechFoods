using FastTechFoods.AuthService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FastTechFoods.AuthService.Infrastructure.Data;

public class AuthDbContext : DbContext
{
    public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
}
