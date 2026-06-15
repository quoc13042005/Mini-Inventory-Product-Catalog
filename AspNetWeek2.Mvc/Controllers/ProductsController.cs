using Microsoft.AspNetCore.Mvc;
using AspNetWeek2.Mvc.Services;

namespace AspNetWeek2.Mvc.Controllers;

public class ProductsController : Controller
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    public async Task<IActionResult> Index()
    {
        var products = await _productService.GetProductListAsync();
        return View(products);
    }

    public async Task<IActionResult> Detail(int id)
    {
        var product = await _productService.GetProductDetailAsync(id);
        if (product == null) return NotFound();
        return View(product);
    }
}
