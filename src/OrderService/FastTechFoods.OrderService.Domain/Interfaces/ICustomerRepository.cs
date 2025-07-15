using FastTechFoods.OrderService.Domain.Entities;

namespace FastTechFoods.OrderService.Domain.Interfaces;

public interface ICustomerRepository
{
    Task<User?> GetByIdAsync(Guid id);

}
