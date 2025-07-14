using FastTechFoods.OrderService.Domain.Interfaces;
using MediatR;

namespace FastTechFoods.OrderService.Application.Commands.CancelOrder;

public class CancelOrderHandler : IRequestHandler<CancelOrderCommand, Unit>
{
    private readonly IOrderRepository _repo;

    public CancelOrderHandler(IOrderRepository repo)
    {
        _repo = repo;
    }

    public async Task<Unit> Handle(CancelOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await _repo.GetByIdAsync(request.OrderId)
                    ?? throw new KeyNotFoundException($"Pedido {request.OrderId} não encontrado.");

        if (order.Status != "Recebido")
            throw new InvalidOperationException("Só é possível cancelar o pedido antes de iniciado o preparo.");

        order.Status = "Cancelado";
        order.CancelReason = request.Reason;

        await _repo.UpdateAsync(order);

        return Unit.Value;
    }
}
