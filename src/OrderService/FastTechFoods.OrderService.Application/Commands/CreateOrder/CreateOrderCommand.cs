using MediatR;

namespace FastTechFoods.OrderService.Application.Commands.CreateOrder;

public record CreateOrderItemDto(Guid ProductId, string ProductName, decimal UnitPrice, int Quantity);

public record CreateOrderCommand(
    Guid ClientId,
    string DeliveryMethod,
    List<CreateOrderItemDto> Items
) : IRequest<Guid>;
