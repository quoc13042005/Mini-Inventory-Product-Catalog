using Microsoft.AspNetCore.Mvc;
using AspNetWeek2.Mvc.Data;
using Microsoft.EntityFrameworkCore;

namespace AspNetWeek2.Mvc.Controllers;

public class CategoriesController : Controller
{
    private readonly AppDbContext _context;

    public CategoriesController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var categories = await _context.Categories
            .Include(c => c.Products)
            .AsNoTracking()
            .ToListAsync();
        return View(categories);
    }
}
