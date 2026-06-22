using Microsoft.AspNetCore.Mvc;

namespace AspNetWeek2.Mvc.Controllers;

public class AuditLogsController : Controller
{
    public IActionResult Index()
    {
        // Simple implementation: read from the Serilog file
        var logFilePath = Path.Combine(Directory.GetCurrentDirectory(), "logs");
        var logFiles = Directory.Exists(logFilePath) ? Directory.GetFiles(logFilePath, "lab05-*.txt") : Array.Empty<string>();
        
        var logs = new List<string>();
        if (logFiles.Any())
        {
            var latestLogFile = logFiles.OrderByDescending(f => f).First();
            logs = System.IO.File.ReadAllLines(latestLogFile).Reverse().Take(100).ToList();
        }

        return View(logs);
    }
}
