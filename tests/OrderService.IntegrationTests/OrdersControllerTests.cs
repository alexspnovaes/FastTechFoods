using FastTechFoods.BuildingBlocks.Messaging.Interfaces;
using FastTechFoods.IntegrationTests.Helpers;
using FastTechFoods.OrderService.Application.Commands.CreateOrder;
using FastTechFoods.OrderService.Domain.Entities;
using FastTechFoods.OrderService.Domain.Interfaces;
using FastTechFoods.OrderService.Infrastructure.Data;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Json;

namespace FastTechFoods.OrderService.IntegrationTests
{
    public class OrdersControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private static readonly Guid _testClientId =
            Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");

        public OrdersControllerTests(WebApplicationFactory<Program> factory)
        {
            var customized = factory.WithWebHostBuilder(builder =>
            {
                builder.UseEnvironment("Testing");

                builder.ConfigureServices(services =>
                {
                    var custDesc = services.SingleOrDefault(d =>
                        d.ServiceType == typeof(ICustomerRepository));
                    if (custDesc != null) services.Remove(custDesc);
                    services.AddScoped<ICustomerRepository, FakeCustomerRepository>();

                    var busDesc = services.SingleOrDefault(d =>
                    d.ServiceType == typeof(IMessageBus));
                    if (busDesc != null) services.Remove(busDesc);
                    services.AddSingleton<IMessageBus, FakeMessageBus>();
                });
            });

            _client = customized.CreateClient();
            var token = TestTokenGenerator.GenerateJwtToken(_testClientId, "client");
            _client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }

        [Fact]
        public async Task Create_GetById_Cancel_Flow_Works()
        {
            var createCmd = new CreateOrderCommand(
                ClientId: _testClientId,
                DeliveryMethod: "Delivery",
                Items:
                [
                    new CreateOrderItemDto(
                        Guid.Parse("11111111-1111-1111-1111-111111111111"),
                        "Produto Genérico",
                        10m,
                        2
                    )
                ]
            );

            var createResp = await _client.PostAsJsonAsync("/api/orders", createCmd);
            createResp.StatusCode.Should().Be(HttpStatusCode.Created);
            var location = createResp.Headers.Location!;

            var getResp = await _client.GetAsync(location);
            getResp.StatusCode.Should().Be(HttpStatusCode.OK);
            var order = await getResp.Content.ReadFromJsonAsync<Order>();
            order.Total.Should().Be(20m);

            var cancelResp = await _client.PostAsJsonAsync($"{location}/cancel", "Cliente desistiu");
            cancelResp.StatusCode.Should().Be(HttpStatusCode.NoContent);

            var get2 = await _client.GetAsync(location);
            var order2 = await get2.Content.ReadFromJsonAsync<Order>();
            order2.Status.Should().Be("Cancelado");
        }

        [Fact]
        public async Task Cancel_AfterCancelOrPrepare_ReturnsBadRequest()
        {
            var cmd = new CreateOrderCommand(
                ClientId: _testClientId,
                DeliveryMethod: "Balcão",
                Items:
                [
                    new CreateOrderItemDto(
                        Guid.Parse("11111111-1111-1111-1111-111111111111"),
                        "Produto Genérico",
                        10m,
                        2
                    )
                ]
            );

            var creater = await _client.PostAsJsonAsync("/api/orders", cmd);
            var location = creater.Headers.Location!;
            await _client.PostAsJsonAsync($"{location}/cancel", "teste");

            var resp = await _client.PostAsJsonAsync($"{location}/cancel", "nova razão");
            resp.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        private class FakeCustomerRepository : ICustomerRepository
        {
            public Task<User?> GetByIdAsync(Guid id) =>
                Task.FromResult<User?>(new User { Id = id, Name = "Cliente Teste" });
        }

        private class FakeMessageBus : IMessageBus
        {
            public Task PublishAsync<T>(string topicOrQueue, T message)
            {
                return Task.CompletedTask;
            }
        }
    }
}
