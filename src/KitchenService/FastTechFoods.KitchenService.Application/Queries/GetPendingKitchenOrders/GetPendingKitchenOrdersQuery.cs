using FastTechFoods.KitchenService.Domain.Entities;
using MediatR;

namespace FastTechFoods.KitchenService.Application.Queries.GetPendingKitchenOrders;

public class GetPendingKitchenOrdersQuery : IRequest<IEnumerable<KitchenOrder>> { }
