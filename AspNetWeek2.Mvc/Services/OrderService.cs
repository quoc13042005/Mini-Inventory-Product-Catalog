using AspNetWeek2.Mvc.Data;
using AspNetWeek2.Mvc.Models;
using AspNetWeek2.Mvc.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace AspNetWeek2.Mvc.Services;

public class OrderService : IOrderService
{
    private readonly AppDbContext _context;

    public OrderService(AppDbContext context)
    {
        _context = context;
    }

    public async Task CreateOrderAsync(OrderCreateViewModel model)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == model.ProductId);
            if (product == null) throw new Exception("Product not found");
            if (product.StockQuantity < model.Quantity) throw new Exception("Not enough stock");

            var order = new Order
            {
                CreatedAt = DateTime.Now,
                TotalAmount = product.Price * model.Quantity
            };
            
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            var item = new OrderItem
            {
                OrderId = order.Id,
                ProductId = product.Id,
                Quantity = model.Quantity,
                UnitPrice = product.Price
            };
            
            _context.OrderItems.Add(item);
            
            product.StockQuantity -= model.Quantity;
            
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}
