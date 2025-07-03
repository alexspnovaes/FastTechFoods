using FastTechFoods.KitchenService.Domain.Entities;
using FastTechFoods.KitchenService.Domain.Interfaces;
using MediatR;

namespace FastTechFoods.KitchenService.Application.Queries.GetKitchenOrderById;

public class GetKitchenOrderByIdHandler : IRequestHandler<GetKitchenOrderByIdQuery, KitchenOrder?>
{
    private readonly IKitchenOrderRepository _repo;

    public GetKitchenOrderByIdHandler(IKitchenOrderRepository repo)
    {
        _repo = repo;
    }

    public async Task<KitchenOrder?> Handle(GetKitchenOrderByIdQuery request, CancellationToken cancellationToken)
    {
        return await _repo.GetByIdAsync(request.OrderId);
    }
}
