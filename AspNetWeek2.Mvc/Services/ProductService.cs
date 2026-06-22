using AspNetWeek2.Mvc.Repositories;
using AspNetWeek2.Mvc.Options;
using AspNetWeek2.Mvc.ViewModels;
using Microsoft.Extensions.Options;

namespace AspNetWeek2.Mvc.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly AppSettings _settings;

    public ProductService(IProductRepository productRepository, IOptions<AppSettings> options)
    {
        _productRepository = productRepository;
        _settings = options.Value;
    }

    public async Task<List<ProductListItemViewModel>> GetProductListAsync()
    {
        var products = await _productRepository.GetAllReadOnlyAsync();
        return products.Select(p => new ProductListItemViewModel
        {
            Id = p.Id,
            Name = p.Name,
            Price = p.Price,
            StockQuantity = p.StockQuantity,
            CategoryName = p.Category != null ? p.Category.Name : "N/A"
        }).ToList();
    }

    public async Task<ProductDetailViewModel?> GetProductDetailAsync(int id)
    {
        var p = await _productRepository.GetByIdAsync(id);
        if (p == null) return null;

        return new ProductDetailViewModel
        {
            Id = p.Id,
            Sku = $"SKU-{p.Id:000}",
            Name = p.Name,
            Category = p.Category?.Name ?? "N/A",
            Supplier = "MiniShop Supplier",
            UnitPrice = p.Price,
            Quantity = p.StockQuantity,
            MinStock = 5,
            LastUpdatedAt = DateTime.Now
        };
    }
}
