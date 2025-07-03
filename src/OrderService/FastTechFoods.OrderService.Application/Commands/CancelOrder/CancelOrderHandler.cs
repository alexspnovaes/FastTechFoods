using FastTechFoods.OrderService.Domain.Interfaces;
using MediatR;

namespace FastTechFoods.OrderService.Application.Commands.CancelOrder;

public class CancelOrderHandler : IRequestHandler<CancelOrderCommand>
{
    private readonly IOrderRepository _repo;

    public CancelOrderHandler(IOrderRepository repo)
    {
        _repo = repo;
    }

    public async Task Handle(CancelOrderCommand request, CancellationToken cancellationToken)
    {
        await _repo.CancelAsync(request.OrderId, request.Reason);
    }
}
