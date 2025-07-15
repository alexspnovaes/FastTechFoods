using MediatR;

namespace FastTechFoods.KitchenService.Application.Commands.AcceptOrder;

public record AcceptOrderCommand(Guid OrderId) : IRequest;
