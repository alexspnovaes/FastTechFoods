using FastTechFoods.KitchenService.Domain.Interfaces;
using MediatR;

namespace FastTechFoods.KitchenService.Application.Commands.AcceptOrder;

public class AcceptOrderHandler : IRequestHandler<AcceptOrderCommand>
{
    private readonly IKitchenOrderRepository _repo;

    public AcceptOrderHandler(IKitchenOrderRepository repo)
    {
        _repo = repo;
    }

    public async Task Handle(AcceptOrderCommand request, CancellationToken cancellationToken)
    {
        await _repo.AcceptAsync(request.OrderId);
    }
}
