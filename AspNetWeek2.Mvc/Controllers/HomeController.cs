using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AspNetWeek2.Mvc.Models;

namespace AspNetWeek2.Mvc.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    public IActionResult TestError()
    {
        throw new Exception("Đây là lỗi giả lập để test Production Error!");
    }
}
