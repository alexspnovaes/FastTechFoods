using FastTechFoods.OrderService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FastTechFoods.OrderService.Infrastructure.Data;

public class OrderDbContext : DbContext
{
    public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options) { }

    public DbSet<Order> Orders => Set<Order>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>().OwnsMany(o => o.Items, a =>
        {
            a.WithOwner().HasForeignKey("OrderId");
            a.Property<Guid>("Id");
            a.HasKey("Id");
            a.Property(i => i.UnitPrice)
             .HasPrecision(18, 2);
        });
    }
}
