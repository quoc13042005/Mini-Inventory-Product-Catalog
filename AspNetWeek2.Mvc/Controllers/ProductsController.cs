using AspNetWeek2.Mvc.Models;
using AspNetWeek2.Mvc.Services;
using AspNetWeek2.Mvc.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AspNetWeek2.Mvc.Controllers;

public class ProductsController : Controller
{
    private readonly ProductService _productService;

    public ProductsController(ProductService productService)
    {
        _productService = productService;
    }

    public IActionResult Index()
    {
        var products = _productService.GetAll()
            .Select(ToListItemViewModel)
            .ToList();

        return View(products);
    }

    public IActionResult Detail(int id)
    {
        var product = _productService.GetById(id);

        if (product == null)
        {
            return NotFound($"Không tìm thấy sản phẩm có id = {id}");
        }

        var viewModel = ToDetailViewModel(product);

        return View(viewModel);
    }

    public IActionResult Stats()
    {
        var stats = _productService.GetStats();

        return View(stats);
    }

    public IActionResult Welcome()
    {
        return Content("Welcome to ASP.NET Core MVC Lab02");
    }

    public IActionResult ProductJson()
    {
        var products = _productService.GetAll()
            .Select(product => new
            {
                product.Id,
                product.Sku,
                product.Name,
                product.Category,
                product.UnitPrice,
                product.Quantity
            });

        return Json(products);
    }

    public IActionResult GoToList()
    {
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Force404()
    {
        return NotFound("Đây là response 404 demo từ action Force404.");
    }

    [HttpGet]
    public IActionResult Search(string? keyword, decimal? minPrice)
    {
        var products = _productService.Search(keyword, minPrice)
            .Select(ToListItemViewModel)
            .ToList();

        var viewModel = new ProductSearchViewModel
        {
            Keyword = keyword ?? "",
            MinPrice = minPrice,
            Products = products
        };

        return View(viewModel);
    }

    [HttpGet]
    public IActionResult Create()
    {
        var viewModel = new ProductCreateViewModel
        {
            Quantity = 1,
            MinStock = 1
        };

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(ProductCreateViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        _productService.Create(model);

        TempData["SuccessMessage"] = "Đã thêm sản phẩm thành công.";

        return RedirectToAction(nameof(Index));
    }

    private static ProductListItemViewModel ToListItemViewModel(Product product)
    {
        return new ProductListItemViewModel
        {
            Id = product.Id,
            Sku = product.Sku,
            Name = product.Name,
            Category = product.Category,
            Supplier = product.Supplier,
            UnitPrice = product.UnitPrice,
            Quantity = product.Quantity,
            MinStock = product.MinStock
        };
    }

    private static ProductDetailViewModel ToDetailViewModel(Product product)
    {
        return new ProductDetailViewModel
        {
            Id = product.Id,
            Sku = product.Sku,
            Name = product.Name,
            Category = product.Category,
            Supplier = product.Supplier,
            UnitPrice = product.UnitPrice,
            Quantity = product.Quantity,
            MinStock = product.MinStock,
            LastUpdatedAt = product.LastUpdatedAt
        };
    }
}
