using Microsoft.AspNetCore.Mvc;
using AspNetWeek2.Mvc.Services;
using AspNetWeek2.Mvc.ViewModels;

namespace AspNetWeek2.Mvc.Controllers;

public class OrdersController : Controller
{
    private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(OrderCreateViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            await _orderService.CreateOrderAsync(model);
            TempData["SuccessMessage"] = "Order created successfully!";
            return RedirectToAction("Index", "Products");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(model);
        }
    }
}
