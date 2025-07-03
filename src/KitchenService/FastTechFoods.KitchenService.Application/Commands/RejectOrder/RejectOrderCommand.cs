using MediatR;

namespace FastTechFoods.KitchenService.Application.Commands.RejectOrder;

public record RejectOrderCommand(Guid OrderId, string Reason) : IRequest;
