using FastTechFoods.KitchenService.Domain.Entities;

namespace FastTechFoods.KitchenService.Domain.Interfaces;

public interface IKitchenOrderRepository
{
    Task<IEnumerable<KitchenOrder>> GetPendingAsync();
    Task<KitchenOrder?> GetByIdAsync(Guid id);
    Task AddAsync(KitchenOrder order);
    Task AcceptAsync(Guid id);
    Task RejectAsync(Guid id, string reason);
    Task MarkReadyAsync(Guid id);
}
