using FastTechFoods.OrderService.Domain.Entities;

namespace FastTechFoods.OrderService.Domain.Interfaces;

public interface IOrderRepository
{
    Task<Order?> GetByIdAsync(Guid id);
    Task<IEnumerable<Order>> GetByClientIdAsync(Guid clientId);
    Task AddAsync(Order order);
    Task UpdateAsync(Order order);
    Task CancelAsync(Guid id, string reason);
}
