using AspNetWeek2.Mvc.Models;
using AspNetWeek2.Mvc.Data;

namespace AspNetWeek2.Mvc.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly AppDbContext _context;

    public OrderRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Order order)
        => await _context.Orders.AddAsync(order);

    public Task SaveChangesAsync()
        => _context.SaveChangesAsync();
}
