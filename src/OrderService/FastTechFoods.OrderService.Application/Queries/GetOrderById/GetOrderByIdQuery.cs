using FastTechFoods.OrderService.Domain.Entities;
using MediatR;

namespace FastTechFoods.OrderService.Application.Queries.GetOrderById;

public record GetOrderByIdQuery(Guid Id) : IRequest<Order?>;
