using FastTechFoods.OrderService.Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace OrderService.IntegrationTests;

public class CustomWebApplicationFactory
        : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var descriptor = services
              .SingleOrDefault(d =>
                 d.ServiceType == typeof(DbContextOptions<OrderDbContext>));
            if (descriptor != null)
                services.Remove(descriptor);

            services.AddDbContext<OrderDbContext>(options =>
            {
                options.UseInMemoryDatabase("InMemoryOrdersTestDb");
            });

            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<OrderDbContext>();
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
        });
    }
}
