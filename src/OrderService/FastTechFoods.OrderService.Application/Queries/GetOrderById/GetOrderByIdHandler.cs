using FastTechFoods.OrderService.Domain.Entities;
using FastTechFoods.OrderService.Domain.Interfaces;
using MediatR;

namespace FastTechFoods.OrderService.Application.Queries.GetOrderById;

public class GetOrderByIdHandler : IRequestHandler<GetOrderByIdQuery, Order?>
{
    private readonly IOrderRepository _repo;

    public GetOrderByIdHandler(IOrderRepository repo)
    {
        _repo = repo;
    }

    public async Task<Order?> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        return await _repo.GetByIdAsync(request.Id);
    }
}
