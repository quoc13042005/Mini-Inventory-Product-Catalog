using AspNetWeek2.Mvc.Models;

namespace AspNetWeek2.Mvc.Repositories;

public interface IOrderRepository
{
    Task AddAsync(Order order);
    Task SaveChangesAsync();
}
