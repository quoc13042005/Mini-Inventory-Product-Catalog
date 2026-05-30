using AspNetWeek2.Mvc.Models;
using AspNetWeek2.Mvc.ViewModels;

namespace AspNetWeek2.Mvc.Services;

public class ProductService
{
    private readonly List<Product> _products = new()
    {
        new Product
        {
            Id = 1,
            Sku = "ACC-MOU-001",
            Name = "Wireless Mouse",
            Category = "Accessories",
            Supplier = "Logitech Vietnam",
            UnitPrice = 250000,
            Quantity = 18,
            MinStock = 5,
            LastUpdatedAt = new DateTime(2025, 5, 9, 9, 12, 0)
        },
        new Product
        {
            Id = 2,
            Sku = "ACC-KEY-002",
            Name = "Mechanical Keyboard",
            Category = "Accessories",
            Supplier = "Keychron Distributor",
            UnitPrice = 1350000,
            Quantity = 4,
            MinStock = 5,
            LastUpdatedAt = new DateTime(2025, 5, 9, 9, 12, 0)
        },
        new Product
        {
            Id = 3,
            Sku = "DSP-MON-003",
            Name = "24-inch Full HD Monitor",
            Category = "Display",
            Supplier = "Dell Partner",
            UnitPrice = 3200000,
            Quantity = 0,
            MinStock = 3,
            LastUpdatedAt = new DateTime(2025, 5, 9, 9, 12, 0)
        },
        new Product
        {
            Id = 4,
            Sku = "HUB-USC-004",
            Name = "USB-C Hub 7 in 1",
            Category = "Accessories",
            Supplier = "Anker Vietnam",
            UnitPrice = 790000,
            Quantity = 9,
            MinStock = 4,
            LastUpdatedAt = new DateTime(2025, 5, 9, 9, 12, 0)
        },
        new Product
        {
            Id = 5,
            Sku = "OFF-STN-005",
            Name = "Laptop Stand",
            Category = "Office",
            Supplier = "Local Supplier",
            UnitPrice = 320000,
            Quantity = 2,
            MinStock = 6,
            LastUpdatedAt = new DateTime(2025, 5, 9, 9, 12, 0)
        },
        new Product
        {
            Id = 6,
            Sku = "CAM-WEB-006",
            Name = "Full HD Webcam",
            Category = "Camera",
            Supplier = "AverMedia",
            UnitPrice = 950000,
            Quantity = 7,
            MinStock = 3,
            LastUpdatedAt = new DateTime(2025, 5, 9, 9, 12, 0)
        }
    };

    public List<Product> GetAll()
    {
        return _products;
    }

    public Product? GetById(int id)
    {
        return _products.FirstOrDefault(product => product.Id == id);
    }

    public ProductStatsViewModel GetStats()
    {
        var totalProducts = _products.Count;

        var totalQuantity = _products.Sum(product => product.Quantity);

        var totalInventoryValue = _products.Sum(product =>
            product.UnitPrice * product.Quantity);

        var outOfStockCount = _products.Count(product =>
            product.Quantity <= 0);

        var needReorderCount = _products.Count(product =>
            product.Quantity > 0 && product.Quantity <= product.MinStock);

        return new ProductStatsViewModel
        {
            TotalProducts = totalProducts,
            TotalQuantity = totalQuantity,
            TotalInventoryValue = totalInventoryValue,
            OutOfStockCount = outOfStockCount,
            NeedReorderCount = needReorderCount
        };
    }

    public List<Product> Search(string? keyword, decimal? minPrice)
    {
        var query = _products.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            query = query.Where(product =>
                product.Name.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                product.Category.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                product.Sku.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                product.Supplier.Contains(keyword, StringComparison.OrdinalIgnoreCase));
        }

        if (minPrice.HasValue)
        {
            query = query.Where(product => product.UnitPrice >= minPrice.Value);
        }

        return query.ToList();
    }

    public Product Create(ProductCreateViewModel model)
    {
        var newId = _products.Count == 0
            ? 1
            : _products.Max(product => product.Id) + 1;

        var product = new Product
        {
            Id = newId,
            Sku = model.Sku,
            Name = model.Name,
            Category = model.Category,
            Supplier = model.Supplier,
            UnitPrice = model.UnitPrice,
            Quantity = model.Quantity,
            MinStock = model.MinStock,
            LastUpdatedAt = DateTime.Now
        };

        _products.Add(product);

        return product;
    }
}
