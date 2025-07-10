using System.Text.Json;
using Azure.Messaging.ServiceBus;
using FastTechFoods.BuildingBlocks.Messaging.Events;
using FastTechFoods.KitchenService.Domain.Entities;
using FastTechFoods.KitchenService.Domain.Interfaces;

namespace FastTechFoods.KitchenService.API.Consumers;

public class OrderCreatedConsumer : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ServiceBusProcessor _processor;

    public OrderCreatedConsumer(IServiceScopeFactory scopeFactory, IConfiguration config)
    {
        _scopeFactory = scopeFactory;
        var client = new ServiceBusClient(config.GetConnectionString("ServiceBus"));
        _processor = client.CreateProcessor("order-created", new ServiceBusProcessorOptions());
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _processor.ProcessMessageAsync += async args =>
        {
            var message = args.Message.Body.ToString();
            var evt = JsonSerializer.Deserialize<OrderCreatedEvent>(message);
            if (evt == null) return;

            using var scope = _scopeFactory.CreateScope();
            var repo = scope.ServiceProvider.GetRequiredService<IKitchenOrderRepository>();

            var order = new KitchenOrder
            {
                OriginalOrderId = evt.OrderId,
                ClientName = evt.ClientName,
                DeliveryMethod = evt.DeliveryMethod,
                Status = "Aguardando",
                Items = evt.Items.Select(i => new KitchenOrderItem
                {
                    ProductName = i.ProductName,
                    Quantity = i.Quantity
                }).ToList()
            };

            await repo.AddAsync(order);
            await args.CompleteMessageAsync(args.Message);
        };

        _processor.ProcessErrorAsync += args =>
        {
            Console.WriteLine(args.Exception);
            return Task.CompletedTask;
        };

        return _processor.StartProcessingAsync(stoppingToken);
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        await _processor.StopProcessingAsync(cancellationToken);
        await base.StopAsync(cancellationToken);
    }
}
