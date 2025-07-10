using FastTechFoods.BuildingBlocks.Messaging.Events;
using FastTechFoods.BuildingBlocks.Messaging.Interfaces;
using FastTechFoods.OrderService.Domain.Entities;
using FastTechFoods.OrderService.Domain.Interfaces;
using MediatR;

namespace FastTechFoods.OrderService.Application.Commands.CreateOrder;

public class CreateOrderHandler(IOrderRepository repo, IMessageBus bus, ICustomerRepository customerRepository) : IRequestHandler<CreateOrderCommand, Guid>
{
    private readonly IOrderRepository _repo = repo;
    private readonly IMessageBus _bus = bus;
    private readonly ICustomerRepository _customerRepository = customerRepository;


    public async Task<Guid> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.GetByIdAsync(request.ClientId)
                      ?? throw new KeyNotFoundException($"Cliente '{request.ClientId}' não encontrado.");


        var order = new Order
        {
            ClientId = request.ClientId,
            DeliveryMethod = request.DeliveryMethod,
            Status = "Recebido",
            Items = [.. request.Items.Select(item => new OrderItem
            {
                ProductId = item.ProductId,
                ProductName = item.ProductName,
                UnitPrice = item.UnitPrice,
                Quantity = item.Quantity
            })]
        };

        var eventMessage = new OrderCreatedEvent
        {
            OrderId = order.Id,
            ClientId = order.ClientId,
            ClientName = customer.Name,
            DeliveryMethod = order.DeliveryMethod,
            Items = [.. order.Items.Select(i => new PedidoCriadoItem
            {
                ProductName = i.ProductName,
                Quantity = i.Quantity
            })]
        };

        await _bus.PublishAsync("order-created", eventMessage);

        await _repo.AddAsync(order);
        return order.Id;
    }
}
