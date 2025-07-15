using FastTechFoods.KitchenService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FastTechFoods.KitchenService.Infrastructure.Data;

public class KitchenDbContext : DbContext
{
    public KitchenDbContext(DbContextOptions<KitchenDbContext> options) : base(options) { }

    public DbSet<KitchenOrder> KitchenOrders => Set<KitchenOrder>();
    public DbSet<KitchenOrderItem> KitchenOrderItems => Set<KitchenOrderItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<KitchenOrder>().OwnsMany(k => k.Items, a =>
        {
            a.WithOwner().HasForeignKey("KitchenOrderId");
            a.Property<Guid>("Id");
            a.HasKey("Id");
        });
    }
}
