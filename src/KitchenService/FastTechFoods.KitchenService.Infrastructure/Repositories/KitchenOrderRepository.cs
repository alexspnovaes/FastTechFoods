using FastTechFoods.KitchenService.Domain.Entities;
using FastTechFoods.KitchenService.Domain.Interfaces;
using FastTechFoods.KitchenService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FastTechFoods.KitchenService.Infrastructure.Repositories;

public class KitchenOrderRepository : IKitchenOrderRepository
{
    private readonly KitchenDbContext _context;

    public KitchenOrderRepository(KitchenDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<KitchenOrder>> GetPendingAsync()
        => await _context.KitchenOrders
            .Include(k => k.Items)
            .Where(k => k.Status == "Aguardando")
            .ToListAsync();

    public async Task<KitchenOrder?> GetByIdAsync(Guid id)
        => await _context.KitchenOrders
            .Include(k => k.Items)
            .FirstOrDefaultAsync(k => k.Id == id);

    public async Task AddAsync(KitchenOrder order)
    {
        _context.KitchenOrders.Add(order);
        await _context.SaveChangesAsync();
    }

    public async Task AcceptAsync(Guid id)
    {
        var order = await GetByIdAsync(id);
        if (order is null) return;

        order.Status = "Aceito";
        await _context.SaveChangesAsync();
    }

    public async Task RejectAsync(Guid id, string reason)
    {
        var order = await GetByIdAsync(id);
        if (order is null) return;

        order.Status = "Rejeitado";
        order.RejectionReason = reason;
        await _context.SaveChangesAsync();
    }

    public async Task MarkReadyAsync(Guid id)
    {
        var order = await GetByIdAsync(id);
        if (order is null) return;

        order.Status = "Pronto";
        await _context.SaveChangesAsync();
    }
}
