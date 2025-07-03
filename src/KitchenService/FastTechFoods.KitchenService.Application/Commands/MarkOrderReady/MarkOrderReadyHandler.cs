using FastTechFoods.KitchenService.Domain.Interfaces;
using MediatR;

namespace FastTechFoods.KitchenService.Application.Commands.MarkOrderReady;

public class MarkOrderReadyHandler : IRequestHandler<MarkOrderReadyCommand>
{
    private readonly IKitchenOrderRepository _repo;

    public MarkOrderReadyHandler(IKitchenOrderRepository repo)
    {
        _repo = repo;
    }

    public async Task Handle(MarkOrderReadyCommand request, CancellationToken cancellationToken)
    {
        await _repo.MarkReadyAsync(request.OrderId);
    }
}
