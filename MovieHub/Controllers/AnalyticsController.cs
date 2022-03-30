using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MovieHub.Controllers;

[Authorize]
public class AnalyticsController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}