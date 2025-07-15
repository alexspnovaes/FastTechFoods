using FastTechFoods.OrderService.Domain.Entities;
using FastTechFoods.OrderService.Domain.Interfaces;
using MediatR;

namespace FastTechFoods.OrderService.Application.Queries.GetOrdersByClientId;

public class GetOrdersByClientIdHandler : IRequestHandler<GetOrdersByClientIdQuery, IEnumerable<Order>>
{
    private readonly IOrderRepository _repo;

    public GetOrdersByClientIdHandler(IOrderRepository repo)
    {
        _repo = repo;
    }

    public async Task<IEnumerable<Order>> Handle(GetOrdersByClientIdQuery request, CancellationToken cancellationToken)
    {
        return await _repo.GetByClientIdAsync(request.ClientId);
    }
}
