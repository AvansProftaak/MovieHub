using Microsoft.AspNetCore.Mvc;

namespace MovieHub.Controllers;

public class AnalyticsController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}