using FastTechFoods.KitchenService.Domain.Entities;
using FastTechFoods.KitchenService.Domain.Interfaces;
using MediatR;

namespace FastTechFoods.KitchenService.Application.Queries.GetPendingKitchenOrders;

public class GetPendingKitchenOrdersHandler : IRequestHandler<GetPendingKitchenOrdersQuery, IEnumerable<KitchenOrder>>
{
    private readonly IKitchenOrderRepository _repo;

    public GetPendingKitchenOrdersHandler(IKitchenOrderRepository repo)
    {
        _repo = repo;
    }

    public async Task<IEnumerable<KitchenOrder>> Handle(GetPendingKitchenOrdersQuery request, CancellationToken cancellationToken)
    {
        return await _repo.GetPendingAsync();
    }
}
