using FastTechFoods.BuildingBlocks.Messaging.Interfaces;
using FastTechFoods.IntegrationTests.Helpers;
using FastTechFoods.KitchenService.API.Consumers;
using FastTechFoods.KitchenService.Domain.Entities;
using FastTechFoods.KitchenService.Infrastructure.Data;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace KitchenService.IntegrationTests;

public class KitchenControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly IServiceScopeFactory _scopeFactory;
    private static readonly Guid _pendingOrderId =
        Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");

    public KitchenControllerTests(WebApplicationFactory<Program> factory)
    {
        var customized = factory.WithWebHostBuilder(builder =>
        {
            builder.UseEnvironment("Testing")
            .ConfigureServices(services =>
                   {
                       var consDesc = services
                               .SingleOrDefault(d => d.ImplementationType == typeof(OrderCreatedConsumer));
                       if (consDesc != null) services.Remove(consDesc);

                       var busDesc = services
                               .SingleOrDefault(d => d.ServiceType == typeof(IMessageBus));
                       if (busDesc != null) services.Remove(busDesc);
                       services.AddSingleton<IMessageBus, FakeMessageBus>();
                   });
        });

        _scopeFactory = customized.Services.GetRequiredService<IServiceScopeFactory>();
        using (var seedScope = _scopeFactory.CreateScope())
        {
            var seedDb = seedScope.ServiceProvider.GetRequiredService<KitchenDbContext>();
            seedDb.Database.EnsureDeleted();
            seedDb.Database.EnsureCreated();
            seedDb.KitchenOrders.Add(new KitchenOrder
            {
                Id = _pendingOrderId,
                OriginalOrderId = Guid.NewGuid(),
                ClientName = "Cliente X",
                DeliveryMethod = "Delivery",
                Items = [new() { ProductName = "Item A", Quantity = 1 }],
                Status = "Aguardando"
            });
            seedDb.SaveChanges();
        }

        _client = customized.CreateClient();
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer",
                TestTokenGenerator.GenerateJwtToken(Guid.NewGuid(), "kitchen"));
    }

    [Fact]
    public async Task GetPending_ReturnsSeededOrder()
    {
        var resp = await _client.GetAsync("/api/kitchen/pending");
        resp.StatusCode.Should().Be(HttpStatusCode.OK);

        var list = await resp.Content.ReadFromJsonAsync<List<KitchenOrder>>();
        list.Should().ContainSingle(o => o.Id == _pendingOrderId);
    }

    [Fact]
    public async Task AcceptOrder_UpdatesStatus()
    {
        var resp = await _client.PostAsync($"/api/kitchen/{_pendingOrderId}/accept", null);
        resp.StatusCode.Should().Be(HttpStatusCode.NoContent);

        using var assertScope = _scopeFactory.CreateScope();
        var db = assertScope.ServiceProvider.GetRequiredService<KitchenDbContext>();
        var updated = await db.KitchenOrders.FindAsync(_pendingOrderId);
        updated.Status.Should().Be("Aceito");
    }

    [Fact]
    public async Task RejectOrder_UpdatesStatusAndReason()
    {
        var resp = await _client.PostAsJsonAsync($"/api/kitchen/{_pendingOrderId}/reject", "Erro");
        resp.StatusCode.Should().Be(HttpStatusCode.NoContent);

        using var assertScope = _scopeFactory.CreateScope();
        var db = assertScope.ServiceProvider.GetRequiredService<KitchenDbContext>();
        var updated = await db.KitchenOrders.FindAsync(_pendingOrderId);
        updated.Status.Should().Be("Rejeitado");
        updated.RejectionReason.Should().Be("Erro");
    }

    [Fact]
    public async Task MarkReady_UpdatesStatus()
    {
        var resp = await _client.PostAsync($"/api/kitchen/{_pendingOrderId}/ready", null);
        resp.StatusCode.Should().Be(HttpStatusCode.NoContent);

        using var assertScope = _scopeFactory.CreateScope();
        var db = assertScope.ServiceProvider.GetRequiredService<KitchenDbContext>();
        var updated = await db.KitchenOrders.FindAsync(_pendingOrderId);
        updated.Status.Should().Be("Pronto");
    }

    private class FakeMessageBus : IMessageBus
    {
        public Task PublishAsync<T>(string topicOrQueue, T message) =>
            Task.CompletedTask;
    }
}