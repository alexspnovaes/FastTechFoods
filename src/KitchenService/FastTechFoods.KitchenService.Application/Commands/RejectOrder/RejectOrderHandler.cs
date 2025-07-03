using FastTechFoods.KitchenService.Domain.Interfaces;
using MediatR;

namespace FastTechFoods.KitchenService.Application.Commands.RejectOrder;

public class RejectOrderHandler : IRequestHandler<RejectOrderCommand>
{
    private readonly IKitchenOrderRepository _repo;

    public RejectOrderHandler(IKitchenOrderRepository repo)
    {
        _repo = repo;
    }

    public async Task Handle(RejectOrderCommand request, CancellationToken cancellationToken)
    {
        await _repo.RejectAsync(request.OrderId, request.Reason);
    }
}
