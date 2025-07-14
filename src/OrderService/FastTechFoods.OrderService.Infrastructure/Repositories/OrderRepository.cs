using FastTechFoods.OrderService.Domain.Entities;
using FastTechFoods.OrderService.Domain.Interfaces;
using FastTechFoods.OrderService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FastTechFoods.OrderService.Infrastructure.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly OrderDbContext _context;

    public OrderRepository(OrderDbContext context)
    {
        _context = context;
    }

    public async Task<Order?> GetByIdAsync(Guid id)
        => await _context.Orders
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.Id == id);

    public async Task<IEnumerable<Order>> GetByClientIdAsync(Guid clientId)
        => await _context.Orders
            .Include(o => o.Items)
            .Where(o => o.ClientId == clientId)
            .ToListAsync();

    public async Task AddAsync(Order order)
    {
        _context.Orders.Add(order);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Order order)
    {
        _context.Orders.Update(order);
        await _context.SaveChangesAsync();
    }  
}
