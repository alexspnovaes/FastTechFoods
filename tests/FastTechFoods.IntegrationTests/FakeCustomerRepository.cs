using FastTechFoods.OrderService.Domain.Entities;
using FastTechFoods.OrderService.Domain.Interfaces;

namespace FastTechFoods.IntegrationTests;

public class FakeCustomerRepository : ICustomerRepository
{
    public Task<User?> GetByIdAsync(Guid id)
    {
        return Task.FromResult<User?>(new User
        {
            Id = id,
            Name = "Cliente Teste"
        });
    }
}
