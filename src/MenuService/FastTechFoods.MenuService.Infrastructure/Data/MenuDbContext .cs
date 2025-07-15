using FastTechFoods.MenuService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FastTechFoods.MenuService.Infrastructure.Data;

public class MenuDbContext : DbContext
{
    public MenuDbContext(DbContextOptions<MenuDbContext> options) : base(options) { }
    public DbSet<Product> Products => Set<Product>();
}
