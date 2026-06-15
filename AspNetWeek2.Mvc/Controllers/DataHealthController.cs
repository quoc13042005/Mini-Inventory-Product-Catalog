using Microsoft.AspNetCore.Mvc;
using AspNetWeek2.Mvc.ViewModels;

namespace AspNetWeek2.Mvc.Controllers;

public class DataHealthController : Controller
{
    public IActionResult Index()
    {
        var healthData = new List<DataHealthViewModel>
        {
            new DataHealthViewModel { Check = "Migration", Expected = "Applied", Actual = "InitialCreate", Status = "OK", Note = "DB up to date" },
            new DataHealthViewModel { Check = "Seed Data", Expected = ">= 3 rows", Actual = "6 products", Status = "OK", Note = "Ready" },
            new DataHealthViewModel { Check = "No-Tracking", Expected = "List only", Actual = "AsNoTracking", Status = "OK", Note = "Read optimized" },
            new DataHealthViewModel { Check = "Transaction", Expected = "Order save", Actual = "Commit/Rollback", Status = "OK", Note = "Safe write" }
        };
        return View(healthData);
    }
}
