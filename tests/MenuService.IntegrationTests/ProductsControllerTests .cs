using FastTechFoods.IntegrationTests.Helpers;
using FastTechFoods.MenuService.Domain.Entities;
using FastTechFoods.MenuService.Infrastructure.Data;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace MenuService.UnitTests;

public class ProductsControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly IServiceScopeFactory _scopeFactory;
    private static readonly Guid _seededId = Guid.Parse("22222222-2222-2222-2222-222222222222");

    public ProductsControllerTests(WebApplicationFactory<Program> factory)
    {
        var customized = factory.WithWebHostBuilder(builder =>
        {
            builder.UseEnvironment("Testing")
                   .ConfigureTestServices(services =>
                   {
                       services.RemoveAll<DbContextOptions<MenuDbContext>>();
                       services.AddDbContext<MenuDbContext>(opt =>
                           opt.UseInMemoryDatabase("TestMenuDb"));
                   });
        });

        _scopeFactory = customized.Services.GetRequiredService<IServiceScopeFactory>();
        using (var seedScope = _scopeFactory.CreateScope())
        {
            var db = seedScope.ServiceProvider.GetRequiredService<MenuDbContext>();
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
            db.Products.Add(new Product
            {
                Id = _seededId,
                Name = "Seeded",
                Description = "Desc",
                Price = 9.99m,
                Category = "Cat",
                IsAvailable = true
            });
            db.SaveChanges();

        }

        _client = customized.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer",
                TestTokenGenerator.GenerateJwtToken(Guid.NewGuid(), "manager"));
    }

    [Fact]
    public async Task GetAll_ReturnsSeededProduct()
    {
        var resp = await _client.GetAsync("/api/products");
        resp.StatusCode.Should().Be(HttpStatusCode.OK);

        var list = await resp.Content.ReadFromJsonAsync<List<Product>>();
        list.Should().ContainSingle(p => p.Id == _seededId && p.Name == "Seeded");
    }

    [Fact]
    public async Task Post_CreatesProduct()
    {
        var cmd = new
        {
            Name = "New",
            Description = "D",
            Price = 5.5m,
            Category = "X"
        };

        var resp = await _client.PostAsJsonAsync("/api/products", cmd);
        resp.StatusCode.Should().Be(HttpStatusCode.Created);

        using var assertScope = _scopeFactory.CreateScope();
        var db = assertScope.ServiceProvider.GetRequiredService<MenuDbContext>();
        db.Products.Should().Contain(p => p.Name == "New" && p.Price == 5.5m);
    }

    [Fact]
    public async Task Put_UpdatesExistingProduct()
    {
        var update = new
        {
            Id = _seededId,      
            Name = "Updated",
            Description = "U",
            Price = 7.7m,
            Category = "Y",
            IsAvailable = false
        };

        var resp = await _client.PutAsJsonAsync($"/api/products/{_seededId}", update);
        resp.StatusCode.Should().Be(HttpStatusCode.NoContent);

        using var assertScope = _scopeFactory.CreateScope();
        var db = assertScope.ServiceProvider.GetRequiredService<MenuDbContext>();
        var prod = await db.Products.FindAsync(_seededId);
        prod.Name.Should().Be("Updated");
        prod.IsAvailable.Should().BeFalse();
    }

    [Fact]
    public async Task Delete_RemovesProduct()
    {
        var resp = await _client.DeleteAsync($"/api/products/{_seededId}");
        resp.StatusCode.Should().Be(HttpStatusCode.NoContent);

        using var assertScope = _scopeFactory.CreateScope();
        var db = assertScope.ServiceProvider.GetRequiredService<MenuDbContext>();
        var prod = await db.Products.FindAsync(_seededId);
        prod.Should().BeNull();
    }
}
