using FastTechFoods.KitchenService.Domain.Entities;
using MediatR;

namespace FastTechFoods.KitchenService.Application.Queries.GetKitchenOrderById;

public record GetKitchenOrderByIdQuery(Guid OrderId) : IRequest<KitchenOrder?>;
