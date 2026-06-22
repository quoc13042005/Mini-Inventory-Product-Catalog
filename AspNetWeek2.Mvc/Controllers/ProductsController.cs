using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AspNetWeek2.Mvc.Data;
using AspNetWeek2.Mvc.Models;
using AspNetWeek2.Mvc.ViewModels;

namespace AspNetWeek2.Mvc.Controllers;

public class ProductsController : Controller
{
    private readonly AppDbContext _context;
    private readonly ILogger<ProductsController> _logger;

    public ProductsController(AppDbContext context, ILogger<ProductsController> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        var products = await _context.Products
            .AsNoTracking()
            .OrderByDescending(p => p.CreatedAt)
            .Select(p => new ProductListItemViewModel
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                StockQuantity = p.StockQuantity,
                CategoryName = p.Category != null ? p.Category.Name : "N/A"
            })
            .ToListAsync();

        return View(products);
    }

    public async Task<IActionResult> Detail(int id)
    {
        var p = await _context.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == id);
        if (p == null) return NotFound();

        var detail = new ProductDetailViewModel
        {
            Id = p.Id,
            Sku = p.SKU,
            Name = p.Name,
            Category = p.Category?.Name ?? "N/A",
            Supplier = "MiniShop Supplier",
            UnitPrice = p.Price,
            Quantity = p.StockQuantity,
            MinStock = 5,
            LastUpdatedAt = p.UpdatedAt ?? p.CreatedAt
        };

        return View(detail);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View(new ProductCreateViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ProductCreateViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var exists = await _context.Products
            .IgnoreQueryFilters()
            .AnyAsync(p => p.SKU == model.SKU);

        if (exists)
        {
            ModelState.AddModelError(nameof(model.SKU), "SKU này đã tồn tại.");
            return View(model);
        }

        var product = new Product
        {
            Name = model.Name,
            SKU = model.SKU,
            Price = model.Price,
            StockQuantity = model.StockQuantity,
            Description = model.Description,
            CreatedAt = DateTime.Now,
            CategoryId = 1 // default category for simplicity since it's required
        };

        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        _logger.LogInformation("Product created. ProductId={ProductId}, SKU={SKU}", product.Id, product.SKU);

        TempData["Success"] = "Đã thêm sản phẩm thành công.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
        if (product == null) return NotFound();

        var model = new ProductEditViewModel
        {
            Id = product.Id,
            Name = product.Name,
            SKU = product.SKU,
            Price = product.Price,
            StockQuantity = product.StockQuantity,
            Description = product.Description,
            RowVersion = product.RowVersion == null ? "" : Convert.ToBase64String(product.RowVersion)
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, ProductEditViewModel model)
    {
        if (id != model.Id) return NotFound();
        if (!ModelState.IsValid) 
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
            _logger.LogWarning("ModelState Invalid in Edit: " + string.Join(", ", errors));
            return View(model);
        }

        var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
        if (product == null) return NotFound();

        product.Name = model.Name;
        product.SKU = model.SKU;
        product.Price = model.Price;
        product.StockQuantity = model.StockQuantity;
        product.Description = model.Description;
        product.UpdatedAt = DateTime.Now;

        byte[]? originalRv = string.IsNullOrEmpty(model.RowVersion) ? null : Convert.FromBase64String(model.RowVersion);
        _context.Entry(product).Property("RowVersion").OriginalValue = originalRv;

        try
        {
            await _context.SaveChangesAsync();
            _logger.LogInformation("Product updated. ProductId={ProductId}", id);
            TempData["Success"] = "Đã cập nhật sản phẩm thành công.";
            return RedirectToAction(nameof(Index));
        }
        catch (DbUpdateConcurrencyException)
        {
            ModelState.AddModelError(string.Empty, "Dữ liệu đã được người khác cập nhật. Vui lòng tải lại trang và thử lại.");
            return View(model);
        }
    }
    
    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
        if (product == null) return NotFound();
        
        // Use ProductDetailViewModel or similar for Delete view
        return View(product);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
        if (product == null) return NotFound();

        product.IsDeleted = true;
        product.DeletedAt = DateTime.Now;
        product.UpdatedAt = DateTime.Now;

        await _context.SaveChangesAsync();
        _logger.LogWarning("Product soft deleted. ProductId={ProductId}", id);

        TempData["Success"] = "Đã xóa mềm sản phẩm.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Trash()
    {
        var deletedProducts = await _context.Products
            .IgnoreQueryFilters()
            .Where(p => p.IsDeleted)
            .AsNoTracking()
            .Select(p => new ProductTrashItemViewModel
            {
                Id = p.Id,
                Name = p.Name,
                DeletedAt = p.DeletedAt
            })
            .ToListAsync();

        return View(deletedProducts);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Restore(int id)
    {
        var product = await _context.Products
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(p => p.Id == id && p.IsDeleted);

        if (product == null) return NotFound();

        product.IsDeleted = false;
        product.DeletedAt = null;
        product.UpdatedAt = DateTime.Now;

        await _context.SaveChangesAsync();
        _logger.LogInformation("Product restored. ProductId={ProductId}", id);
        
        TempData["Success"] = "Đã khôi phục sản phẩm.";
        return RedirectToAction(nameof(Trash));
    }
}
