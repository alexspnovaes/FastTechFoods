using FastTechFoods.OrderService.Domain.Entities;
using FastTechFoods.OrderService.Domain.Interfaces;
using FastTechFoods.OrderService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace FastTechFoods.OrderService.Infrastructure.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly OrderDbContext _context;

    public CustomerRepository(OrderDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByIdAsync(Guid id)
      => await _context.Users
          .FirstOrDefaultAsync(o => o.Id == id);
}
