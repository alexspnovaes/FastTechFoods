using MediatR;

namespace FastTechFoods.KitchenService.Application.Commands.MarkOrderReady;

public record MarkOrderReadyCommand(Guid OrderId) : IRequest;
