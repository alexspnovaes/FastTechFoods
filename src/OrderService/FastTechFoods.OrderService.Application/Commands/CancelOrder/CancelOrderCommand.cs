using MediatR;

namespace FastTechFoods.OrderService.Application.Commands.CancelOrder;

public record CancelOrderCommand(Guid OrderId, string Reason) : IRequest<Unit>;
