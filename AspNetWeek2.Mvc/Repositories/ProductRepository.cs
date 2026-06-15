using Microsoft.EntityFrameworkCore;
using AspNetWeek2.Mvc.Models;
using AspNetWeek2.Mvc.Data;

namespace AspNetWeek2.Mvc.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _context;

    public ProductRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<List<Product>> GetAllAsync()
        => _context.Products.Include(p => p.Category).ToListAsync();

    public Task<List<Product>> GetAllReadOnlyAsync()
        => _context.Products.Include(p => p.Category).AsNoTracking().ToListAsync();

    public Task<Product?> GetByIdAsync(int id)
        => _context.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == id);

    public async Task AddAsync(Product product)
        => await _context.Products.AddAsync(product);

    public Task SaveChangesAsync()
        => _context.SaveChangesAsync();
}
