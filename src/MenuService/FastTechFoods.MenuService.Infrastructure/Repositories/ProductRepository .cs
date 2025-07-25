﻿using FastTechFoods.MenuService.Domain.Entities;
using FastTechFoods.MenuService.Domain.Interfaces;
using FastTechFoods.MenuService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FastTechFoods.MenuService.Infrastructure.Repositories;
public class ProductRepository : IProductRepository
{
    private readonly MenuDbContext _context;

    public ProductRepository(MenuDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Product>> GetAllAsync(string? category = null)
    {
        var query = _context.Products.AsQueryable();

        if (!string.IsNullOrWhiteSpace(category))
        {
            var cat = category.Trim().ToLower();
            query = query.Where(p => p.Category.Equals(cat, StringComparison.CurrentCultureIgnoreCase));
        }

        return await query.ToListAsync();
    }


    public async Task<Product?> GetByIdAsync(Guid id)
        => await _context.Products.FindAsync(id);

    public async Task AddAsync(Product product)
    {
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Product product)
    {
        _context.Products.Update(product);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product is null) return;
        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
    }
}
