using FastTechFoods.OrderService.Domain.Entities;
using MediatR;

namespace FastTechFoods.OrderService.Application.Queries.GetOrdersByClientId;

public record GetOrdersByClientIdQuery(Guid ClientId) : IRequest<IEnumerable<Order>>;
